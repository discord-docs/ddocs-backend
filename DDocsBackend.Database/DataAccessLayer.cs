using DDocsBackend.Data.Context;
using Microsoft.EntityFrameworkCore;
using DDocsBackend.Data.Models;
using Fastenshtein;

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

        public async Task<Event[]> GetEventsAsync(int year)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            return await context.Events.Where(x => x.HeldAt.Year == year).ToArrayAsync().ConfigureAwait(false);
        }

        public async Task<Event?> GetEventAsync(Guid id)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            return await context.Events.Include(x => x.Authors).Include(x => x.Summaries).FirstOrDefaultAsync(x => x.EventId == id).ConfigureAwait(false);
        }

        public async Task<Event[]> SearchEventsAsync(string query)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);


            return await context.Events.Where(p => EF.Functions.ILike(p.Title!, $"%{query}%")).ToArrayAsync().ConfigureAwait(false);
        }

        public async Task CreateEventAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            await context.Events.AddAsync(GenEvent());

            context.SaveChanges();
        }

        private Event GenEvent()
        {
            var id = Guid.NewGuid();

            var ev =  new Event
            {
                Title = "Test",
                Description = "Test desc",
                Deprecated = false,
                EventId = id,
                HeldAt = DateTimeOffset.UtcNow,
                Authors = new List<Author>()
                {
                    new Author
                    {
                        UserId = 259053800755691520,
                        Revised = false
                    },
                    new Author
                    {
                        UserId = 242351388137488384,
                        Revised = true
                    },
                    new Author
                    {
                        UserId = 619241308912877609,
                        Revised = true
                    },
                    new Author
                    {
                        UserId = 840806601957965864,
                        Revised = true
                    }
                },
                Summaries = new List<Summary>
                {
                    new Summary
                    {
                        Content = "Test content",
                        EventId = id,
                        IsNew = true,
                        Status = null,
                        SummaryId = Guid.NewGuid(),
                        Title = "Test title",
                        Type = SummaryType.QNAAnswer,
                        Url = null,
                        LastRevised = DateTimeOffset.UtcNow
                    },
                    new Summary
                    {
                        Content = "Test content",
                        EventId = id,
                        IsNew = false,
                        Status = null,
                        SummaryId = Guid.NewGuid(),
                        Title = "Test title",
                        Type = SummaryType.QNAAnswer,
                        Url = "https://s3-alpha-sig.figma.com/img/2df7/f218/98835dfa567742829fbd6558f88fd217?Expires=1644796800&Signature=PO8t2XjqNMhPb-7SFw0OptWjCMCTMY7e3x3HglpASbxLAVRN1iu0wl3ZRLgrmWdxFW9Fi163sE0qZxwaYPbzzwwlyHqxhz-dKceccgh6spSt~4t2AH2AuyMJY4D0o3QI-z5CP6BBLY7nNdNfwdAovbEYtcBs9I44Rs1mf44WNrW0Uaxres8gMxJWugPbqPuj~RPt58zDLZPa2pw40HFaXx7amyNDv3RS3suaAqer8v17MkeNX1Am1HhF7jCPONQvIRFJnRakYW~fhRxA3AGF4UwL-KFvs0Ki5tIxAOexFrjXNMXwcbKw2VMcEtFHs8lruAQj9GvrHoMZkTe6z9KFTQ__&Key-Pair-Id=APKAINTVSUGEWH5XD5UA",
                        LastRevised = DateTimeOffset.UtcNow
                    },
                    new Summary
                    {
                        Content = "Test content\n# h1\n## h2\n ### h3\n#### h4",
                        EventId = id,
                        IsNew = true,
                        Status = FeatureType.ClosedBeta,
                        SummaryId = Guid.NewGuid(),
                        Title = "Test title",
                        Type = SummaryType.Feature,
                        Url = "https://s3-alpha-sig.figma.com/img/2df7/f218/98835dfa567742829fbd6558f88fd217?Expires=1644796800&Signature=PO8t2XjqNMhPb-7SFw0OptWjCMCTMY7e3x3HglpASbxLAVRN1iu0wl3ZRLgrmWdxFW9Fi163sE0qZxwaYPbzzwwlyHqxhz-dKceccgh6spSt~4t2AH2AuyMJY4D0o3QI-z5CP6BBLY7nNdNfwdAovbEYtcBs9I44Rs1mf44WNrW0Uaxres8gMxJWugPbqPuj~RPt58zDLZPa2pw40HFaXx7amyNDv3RS3suaAqer8v17MkeNX1Am1HhF7jCPONQvIRFJnRakYW~fhRxA3AGF4UwL-KFvs0Ki5tIxAOexFrjXNMXwcbKw2VMcEtFHs8lruAQj9GvrHoMZkTe6z9KFTQ__&Key-Pair-Id=APKAINTVSUGEWH5XD5UA",
                        LastRevised = DateTimeOffset.UtcNow
                    }
                },
                Thumbnail = "https://s3-alpha-sig.figma.com/img/2df7/f218/98835dfa567742829fbd6558f88fd217?Expires=1644796800&Signature=PO8t2XjqNMhPb-7SFw0OptWjCMCTMY7e3x3HglpASbxLAVRN1iu0wl3ZRLgrmWdxFW9Fi163sE0qZxwaYPbzzwwlyHqxhz-dKceccgh6spSt~4t2AH2AuyMJY4D0o3QI-z5CP6BBLY7nNdNfwdAovbEYtcBs9I44Rs1mf44WNrW0Uaxres8gMxJWugPbqPuj~RPt58zDLZPa2pw40HFaXx7amyNDv3RS3suaAqer8v17MkeNX1Am1HhF7jCPONQvIRFJnRakYW~fhRxA3AGF4UwL-KFvs0Ki5tIxAOexFrjXNMXwcbKw2VMcEtFHs8lruAQj9GvrHoMZkTe6z9KFTQ__&Key-Pair-Id=APKAINTVSUGEWH5XD5UA",
            };

            return ev;
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

        public async Task DeleteAuthenticationAsync(Authentication auth)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

            context.Authentication.Remove(auth);

            var discordAuth = await context.DiscordAuthentication.FindAsync(auth.UserId).ConfigureAwait(false);

            if (discordAuth != null) 
                context.DiscordAuthentication.Remove(discordAuth);

            await context.SaveChangesAsync().ConfigureAwait(false);
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