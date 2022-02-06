using DDocsBackend.Data;
using DDocsBackend.Data.Models;
using DDocsBackend.RestModels;
using Discord.Rest;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Services
{
    public class AuthenticationService
    {
        private readonly string _jwtSecret;
        private readonly Logger _log;

        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _urlEncoder;
        private readonly IJwtEncoder _encoder;
        private readonly IJwtDecoder _decoder;
        private readonly IJwtValidator _validator;
        private readonly IDateTimeProvider _provider;

        private readonly DataAccessLayer _dataAccessLayer;

        public AuthenticationService(IConfiguration config, DataAccessLayer dataAccessLayer)
        {
            _log = Logger.GetLogger<AuthenticationService>();

            _dataAccessLayer = dataAccessLayer;

            if (config["JWT_SECRET"] == null)
            {
                _log.Critical("No JWT secret specified in the env variables");
                throw new Exception("No JWT secret specified in the env variables");
            }

            _jwtSecret = config["JWT_SECRET"];

            _algorithm = new HMACSHA256Algorithm();
            _serializer = new JsonConvertSerializer();
            _urlEncoder = new JwtBase64UrlEncoder();
            _provider = new UtcDateTimeProvider();
            _validator = new JwtValidator(_serializer, _provider);
            _encoder = new JwtEncoder(_algorithm, _serializer, _urlEncoder);
            _decoder = new JwtDecoder(_serializer, _validator, _urlEncoder, _algorithm);
        }

        /// <exception cref="TokenExpiredException"></exception>
        /// <exception cref="SignatureVerificationException"></exception>
        public async Task<Authentication?> GetAuthenticationAsync(string jwt)
        {
            var decoded = Decode<JWTPayload?>(jwt);

            if (decoded == null)
                return null;

            if (decoded.ExpiresAt < DateTimeOffset.UtcNow)
                return null;

            return await _dataAccessLayer.GetAuthenticationAsync(decoded.UserId);
        }

        public bool IsValidRefreshToken(string refreshToken, Authentication auth)
        {
            if (auth.JWTRefreshToken != refreshToken)
                return false;

            if (DateTimeOffset.UtcNow > auth.RefreshExpiresAt)
                return false;

            return true;
        }

        public async Task<(string newRefresh, string newJWT)?> ApplyRefreshTokenAsync(string refresh)
        {
            var auth = await _dataAccessLayer.GetAuthenticationAsync(refresh).ConfigureAwait(false);

            if (auth == null)
                return null;

            return await ApplyRefreshTokenAsync(auth).ConfigureAwait(false);
        }

        public async Task<(string newRefresh, string newJWT)?> ApplyRefreshTokenAsync(Authentication auth)
        {
            var newToken = GenerateRefreshToken();

            var result = await _dataAccessLayer.ApplyJWTRefreshAsync(auth.UserId, newToken, DateTime.UtcNow.AddDays(7));

            if (result == null)
                return null;

            var jwt = Encode(new JWTPayload
            {
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5),
                IssuedAt = DateTimeOffset.UtcNow,
                UserId = auth.UserId,
            });

            return (newToken, jwt);
        }

        public async Task<(Authentication Authentication, string Token)> CreateAuthenticationAsync(AccessTokenResponse oauth)
        {
            var refresh = GenerateRefreshToken();

            ulong userId = 0;

            using(var discordClient = new DiscordRestClient())
            {
                await discordClient.LoginAsync(Discord.TokenType.Bearer, oauth.AccessToken);

                userId = discordClient.CurrentUser.Id;
            }

            var auth = await _dataAccessLayer.CreateAuthenticationAsync(
                oauth.AccessToken,
                oauth.RefreshToken,
                DateTimeOffset.UtcNow.AddSeconds(oauth.ExpiresIn),
                refresh,
                DateTimeOffset.UtcNow.AddDays(7),
                userId);

            var jwt = Encode(new JWTPayload
            {
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(10),
                IssuedAt = DateTimeOffset.UtcNow,
                UserId = userId
            });

            return (auth, jwt);
        }

        private string GenerateRefreshToken()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                return $"{Convert.ToBase64String(tokenData)}";
            }
        }

        public string Encode(object payload)
            => _encoder.Encode(payload, _jwtSecret);

        /// <exception cref="TokenExpiredException"></exception>
        /// <exception cref="SignatureVerificationException"></exception>
        public TResult Decode<TResult>(string token)
            => _decoder.DecodeToObject<TResult>(token, _jwtSecret, true);
    }

    public class JsonConvertSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json)!;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
