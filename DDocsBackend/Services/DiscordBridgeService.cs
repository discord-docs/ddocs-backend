using DDocsBackend.Data;
using DDocsBackend.Data.Models;
using DDocsBackend.Helpers;
using Discord;
using Discord.Net;
using Discord.Rest;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Services
{
    public class DiscordBridgeService
    {
        // The number of days an avatar will last before refreshed.
        public const int AvatarLifecyleDuration = 7;

        private DateTimeOffset AvatarExpiration
            => DateTimeOffset.UtcNow.AddDays(-AvatarLifecyleDuration);

        private AuthenticationService _authService;
        private DiscordOAuthHelper _oauthHelper;
        private DataAccessLayer _dataAccessLayer;
        private ObjectCache _cache;
        private readonly string _token;
        private readonly DiscordRestClient _botClient;
        private readonly CDNService _cdn;

        public DiscordBridgeService(CDNService cdn, AuthenticationService authService, DiscordOAuthHelper oauthHelper, DataAccessLayer dataAccessLayer, IConfiguration config)
        {
            _token = config["BOT_TOKEN"];
            _authService = authService;
            _oauthHelper = oauthHelper;
            _dataAccessLayer = dataAccessLayer;
            _cache = MemoryCache.Default;
            _botClient = new();
            _cdn = cdn;
        }

        public async Task<UserDetails> GetUserDetailsAsync(ulong id)
        {
            var cached = _cache.Get($"{id}");

            if(cached != null)
            {
                return (UserDetails)cached!;
            }

            var unknownUser = new UserDetails("Unknown", "0000", id, GetDefaultAvatar(id));

            // load from database, try verified authors
            var verifiedAuthor = await _dataAccessLayer.GetVerifiedAuthorAsync(id).ConfigureAwait(false);

            if(verifiedAuthor != null)
            {
                // check their avatar hash against their user
                var pfp = await _dataAccessLayer.GetAssetAsync($"{id}").ConfigureAwait(false);

                if(pfp != null)
                {
                    // check the avatars creation time and then check our policy for wether or not to try to update it
                    if(pfp.CreatedAt < AvatarExpiration)
                    {
                        // get their user from discord
                        var user = await GetUserAsync(id).ConfigureAwait(false);

                        if(user == null)
                        {
                            // return their previous avatar
                            var tempDetails = new UserDetails(verifiedAuthor.Username!, verifiedAuthor.Discriminator!, verifiedAuthor.UserId, pfp.Id!);
                            // cache for an hour or so?
                            _cache.Set($"{id}", tempDetails, DateTime.UtcNow.AddHours(1));
                            return tempDetails;
                        }

                        // refresh their avatar
                        await CreateAvatarAssetAsync(user, pfp.Id!).ConfigureAwait(false);

                        // modify the create date
                        await _dataAccessLayer.ModifyAssetAsync(pfp.Id!, x => x.CreatedAt = DateTimeOffset.UtcNow).ConfigureAwait(false);

                        // cache it
                        var details = new UserDetails(user, pfp.Id!);
                        _cache.Set($"{id}", details, DateTime.UtcNow.AddDays(1));
                        return details;
                    }
                    else
                    {
                        return new UserDetails(verifiedAuthor.Username!, verifiedAuthor.Discriminator!, verifiedAuthor.UserId, pfp.Id!);
                    }
                }
                else
                {
                    // they don't have an asset, try to get their user and create one

                    // get their user from discord
                    var user = await GetUserAsync(id).ConfigureAwait(false);

                    if(user == null)
                    {
                        // at this point we haven't created an asset for them and we can't get their user info with a bot or with oauth,
                        // lets just return a default avatar
                        _cache.Set($"{id}", unknownUser, DateTime.UtcNow.AddMinutes(10));
                        return unknownUser;
                    }

                    // create their avatar
                    var asset = await _dataAccessLayer.CreateAssetAsync(AssetType.Avatar, ContentType.Png).ConfigureAwait(false);
                    await CreateAvatarAssetAsync(user, asset.Id!).ConfigureAwait(false);

                    // cache it
                    var details = new UserDetails(user, asset.Id!);
                    _cache.Set($"{id}", details, DateTime.UtcNow.AddDays(1));
                    return details;
                }
            }
            else
            {
                // try getting from discord
                var user = await GetUserAsync(id).ConfigureAwait(false);

                if (user == null)
                {
                    // cache for ratelimits
                    _cache.Set($"{id}", unknownUser, DateTime.UtcNow.AddMinutes(10));
                    return unknownUser;
                }

                // create their avatar
                var asset = await _dataAccessLayer.CreateAssetAsync(AssetType.Avatar, ContentType.Png).ConfigureAwait(false);
                await CreateAvatarAssetAsync(user, asset.Id!).ConfigureAwait(false);

                // cache it
                var details = new UserDetails(user, asset.Id!);
                _cache.Set($"{id}", details, DateTime.UtcNow.AddDays(1));
                return details;
            }
        }

        private async Task CreateAvatarAssetAsync(IUser user, string id)
        {
            using(var httpClient = new HttpClient())
            {
                var result = await httpClient.GetAsync(user.GetAvatarUrl(ImageFormat.Png) ?? user.GetDefaultAvatarUrl()).ConfigureAwait(false);

                using (var image = SixLabors.ImageSharp.Image.Load(await result.Content.ReadAsStreamAsync()))
                {
                    await _cdn.WriteAsync(id, AssetType.Avatar, ContentType.Png, image);
                }
            }
        }

        public async ValueTask<IUser?> GetUserAsync(ulong id)
        {
            var cached = _cache.Get($"{id}");

            if (cached != null)
                return cached as IUser;

            if (_botClient.LoginState != LoginState.LoggedIn)
                await _botClient.LoginAsync(TokenType.Bot, _token);

            var user = await _botClient.GetUserAsync(id);

            if (user == null)
            {
                // get with oauth?
                var oauth = await _dataAccessLayer.GetDiscordOAuthAsync(id).ConfigureAwait(false);

                if (oauth == null)
                    return null;

                return await GetUserAsync(oauth).ConfigureAwait(false);
            }

            _cache.Add($"{id}", user, DateTime.UtcNow.AddDays(1));

            return user;
        }

        public async ValueTask<IUser?> GetUserAsync(DiscordOAuthAuthentication auth)
        {
            var cached = _cache.Get($"{auth.UserId}");

            if (cached != null)
                return cached as IUser;

            using (var client = await GetClientAsync(auth))
            {
                if (client == null)
                    return null;

                var user = (IUser)client.CurrentUser;

                _cache.Set($"{auth.UserId}", user, DateTime.UtcNow.AddDays(1));
                return user;
            }
        }

        public string GetDefaultAvatar(ulong userId)
        {
            return $"https://cdn.discordapp.com/embed/avatars/{userId % 5}.png";
        }

        private async Task<DiscordRestClient?> GetClientAsync(DiscordOAuthAuthentication auth)
        {
            var client = new DiscordRestClient();

            if (auth.ExpiresAt < DateTimeOffset.UtcNow && !await RefreshTokenAsync(auth))
                return null;

            try
            {
                await client.LoginAsync(TokenType.Bearer, auth.AccessToken);

                return client;
            }
            catch (HttpException x) when (x.HttpCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (!await RefreshTokenAsync(auth))
                    return null;

                try
                {
                    await client.LoginAsync(TokenType.Bearer, auth.AccessToken);

                    return client;

                }
                catch (HttpException x2) when (x2.HttpCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
            }
        }

        private async Task<bool> RefreshTokenAsync(DiscordOAuthAuthentication auth)
        {
            var result = await _oauthHelper.RefreshTokenAsync(auth.RefreshToken);

            if (result == null)
                return false;

            auth = (await _dataAccessLayer.ApplyDiscordOAuthRefresh(auth.UserId, result.AccessToken, result.RefreshToken, DateTimeOffset.UtcNow.AddSeconds(result.ExpiresIn)))!;
            return true;
        }
    }

    public class UserDetails
    {
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string Avatar { get; set; }
        public ulong UserId { get; set; }

        public UserDetails(IUser user, string avatar)
            : this(user.Username, user.Discriminator, user.Id, avatar) { }

        public UserDetails(string username, string discriminator, ulong userId, string avatar)
        {
            Username = username;
            Discriminator = discriminator;
            Avatar = avatar;
            UserId = userId;
        }
    }
}
