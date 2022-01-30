using DDocsBackend.Data.Context;
using Microsoft.EntityFrameworkCore;
using DDocsBackend.Data.Models;

namespace DDocsBackend.Data
{
    /// <summary>
    ///     The data access layer used by the DDocsBackend service.
    /// </summary>
    public class DataAccessLayer
    {
        private readonly IDbContextFactory<DDocsContext> _contextFactory;

        public DataAccessLayer(IDbContextFactory<DDocsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Author?> GetAuthorAsync(ulong userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            return await context.Authors.FindAsync(userId).ConfigureAwait(false);
        }

        public async Task<Authentication?> GetAuthenticationAsync(ulong userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            return await context.Authentication.FindAsync(userId).ConfigureAwait(false);
        }

        public async Task<Authentication?> GetAuthenticationAsync(string refreshToken)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            return await context.Authentication.FirstOrDefaultAsync(x => x.JWTRefreshToken == refreshToken);
        }

        public async Task<Authentication> CreateAuthenticationAsync(string? discordAccessToken, string? discordRefreshToken, DateTimeOffset discordExpiresAt, string jwtRefreshToken, DateTimeOffset jwtRefrshValidUntil, ulong userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            var entity = await context.AddAsync(new Authentication
            {
                UserId = userId,
                JWTRefreshToken = jwtRefreshToken,
                RefreshExpiresAt = jwtRefrshValidUntil,
                DiscordAuthentication = new DiscordOAuthAuthentication()
                {
                    AccessToken = discordAccessToken,
                    ExpiresAt = discordExpiresAt,
                    RefreshToken = discordRefreshToken,
                    UserId = userId
                }
            });

            await context.SaveChangesAsync();

            return entity.Entity;
        }

        public async Task<Authentication?> ApplyJWTRefreshAsync(ulong userId, string refreshToken, DateTimeOffset expiresAt)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            var authentication = await context.Authentication.FindAsync(userId).ConfigureAwait(false);

            if (authentication == null)
                return null;

            authentication.JWTRefreshToken = refreshToken;
            authentication.RefreshExpiresAt = expiresAt;

            await context.SaveChangesAsync();

            return authentication;
        }
    }
}