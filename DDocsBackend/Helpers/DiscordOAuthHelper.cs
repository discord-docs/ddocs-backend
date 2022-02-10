using DDocsBackend.Data.Models;
using DDocsBackend.RestModels;
using DDocsBackend.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Helpers
{
    public class DiscordOAuthHelper
    {
        private readonly string? _clientId;
        private readonly string? _clientSecret;
        private readonly string? _redirectUri;

        private readonly Logger _log;

        public DiscordOAuthHelper(IConfiguration config)
        {
            _log = Logger.GetLogger<DiscordOAuthHelper>();

            _clientId = config["CLIENT_ID"];
            _clientSecret = config["CLIENT_SECRET"];
            _redirectUri = config["REDIRECT_URI"];

            if (_clientSecret == null && _clientId == null || _redirectUri == null)
            {
                _log.Critical("CLIENT_ID, CLIENT_SECRET, or REDIRECT_URI was not specified in the enviorment variables.");
            }
        }

        public async Task<AccessTokenResponse?> GetTokenAsync(string code)
        {
            using(var client = new HttpClient())
            {
                Dictionary<string, string?> queryString = new();

                queryString.Add("client_id", _clientId);
                queryString.Add("client_secret", _clientSecret);
                queryString.Add("grant_type", "authorization_code");
                queryString.Add("redirect_uri", _redirectUri);
                queryString.Add("code", code);

                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                var result = await client.PostAsync($"https://discord.com/api/oauth2/token", new FormUrlEncodedContent(queryString));

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AccessTokenResponse>(content);
                }

                _log.Warn($"Failed to get oauth token, got code {result.StatusCode} with redir {_redirectUri}");

                return null;
            }
        }

        public async Task<AccessTokenResponse?> RefreshTokenAsync(string? refreshToken)
        {
            using (var client = new HttpClient())
            {
                Dictionary<string, string?> queryString = new();

                queryString.Add("client_id", _clientId);
                queryString.Add("client_secret", _clientSecret);
                queryString.Add("grant_type", "refresh_token");
                queryString.Add("redirect_uri", _redirectUri);
                queryString.Add("refresh_token", refreshToken);

                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                var result = await client.PostAsync($"https://discord.com/api/oauth2/token", new FormUrlEncodedContent(queryString));

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AccessTokenResponse>(content);
                }

                return null;
            }
        }
    }
}
