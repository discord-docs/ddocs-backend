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
        private AuthenticationService _authService;
        private DiscordOAuthHelper _oauthHelper;
        private DataAccessLayer _dataAccessLayer;
        private ObjectCache _cache;
        private readonly string _token;
        private readonly DiscordRestClient _botClient;

        public DiscordBridgeService(AuthenticationService authService, DiscordOAuthHelper oauthHelper, DataAccessLayer dataAccessLayer, IConfiguration config)
        {
            _token = config["BOT_TOKEN"];
            _authService = authService;
            _oauthHelper = oauthHelper;
            _dataAccessLayer = dataAccessLayer;
            _cache = MemoryCache.Default;
            _botClient = new();
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
                return null;

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
}
