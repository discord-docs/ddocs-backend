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

        public async Task<DiscordOAuthAuthentication?> GetDiscordOAuthAsync(ulong userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            return await context.DiscordAuthentication.FindAsync(userId).ConfigureAwait(false);
        }

        public async Task<DiscordOAuthAuthentication?> ApplyDiscordOAuthRefresh(ulong userId, string? newAccessToken, string? newRefreshToken, DateTimeOffset expiresAt)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            var user = await context.DiscordAuthentication.FindAsync(userId).ConfigureAwait(false);

            if (user == null)
                return null;

            user.AccessToken = newAccessToken;
            user.RefreshToken = newRefreshToken;
            user.ExpiresAt = expiresAt;

            await context.SaveChangesAsync().ConfigureAwait(false);

            return user;
        }

        public async Task<bool> IsAuthorAsync(ulong userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            return await context.VerifiedAuthors.AnyAsync(x => x.UserId == userId).ConfigureAwait(false);
        }

        public async Task<Authentication> CreateAuthenticationAsync(string? discordAccessToken, string? discordRefreshToken, DateTimeOffset discordExpiresAt, string jwtRefreshToken, DateTimeOffset jwtRefrshValidUntil, ulong userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            var entity = await context.AddAsync(new Authentication
            {
                UserId = userId,
                JWTRefreshToken = jwtRefreshToken,
                RefreshExpiresAt = jwtRefrshValidUntil,
            }).ConfigureAwait(false);

            await context.AddAsync(new DiscordOAuthAuthentication()
            {
                AccessToken = discordAccessToken,
                ExpiresAt = discordExpiresAt,
                RefreshToken = discordRefreshToken,
                UserId = userId
            }).ConfigureAwait(false);

            await context.SaveChangesAsync().ConfigureAwait(false);

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

            await context.SaveChangesAsync().ConfigureAwait(false);

            return authentication;
        }
    }
}