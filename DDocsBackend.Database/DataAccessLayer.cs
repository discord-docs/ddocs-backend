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
                Title = $"Discord Developer Stage #{new Random().Next(1, 100)}",
                Description = "Test description with some lorem ipsum: Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
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
                    },
                    new Author
                    {
                        UserId = 188363246695219201,
                        Revised = true,
                    },
                    new Author
                    {
                        UserId = 130510525770629121,
                        Revised = true,
                    },
                    new Author
                    {
                        UserId = 545581357812678656,
                        Revised = true
                    }
                },
                Summaries = new List<Summary>
                {
                    new Summary
                    {
                        Content = "Test content with just long text: Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        EventId = id,
                        IsNew = true,
                        Status = null,
                        SummaryId = Guid.NewGuid(),
                        Title = "Long text",
                        Type = SummaryType.QNAAnswer,
                        Url = null,
                        LastRevised = DateTimeOffset.UtcNow
                    },
                    new Summary
                    {
                        Content = "# h1\r\n## h2\r\n### h3\r\n#### h4\r\n##### h5\r\n###### h6\r\n\r\nText\r\n\r\n**Bold**\r\n\r\n*Italic*\r\n\r\n~~Strikethru~~\r\n\r\n[Link](https://discord.com/channels/936725123724111953/937130072551346216/938543687196090428)\r\n\r\n![Image](https://media.discordapp.net/attachments/937130072551346216/938543686432718928/unknown.png?width=1351&height=903)\r\n\r\n```\r\nCode block no lang\r\n```\r\n\r\n```json\r\n{\r\n\t\"lang\": \"json\"\r\n}\r\n```\r\n\r\nBulletpoints:\r\n- A\r\n- B\r\n- C\r\n\r\nNumbered:\r\n1. One\r\n2. Two\r\n3. Three\r\n\r\nDouble indenting\r\n- Level one\r\n\t- Level two\r\n\t\t- Level 3\r\n\t\t\t- Level 4\r\n\r\nQuote\r\n> I said that\r\n\r\nLine\r\n\r\n---\r\n\r\nEnd line",
                        EventId = id,
                        IsNew = false,
                        Status = null,
                        SummaryId = Guid.NewGuid(),
                        Title = "Full markdown test",
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
                        Title = "Just headings",
                        Type = SummaryType.Feature,
                        Url = "https://s3-alpha-sig.figma.com/img/2df7/f218/98835dfa567742829fbd6558f88fd217?Expires=1644796800&Signature=PO8t2XjqNMhPb-7SFw0OptWjCMCTMY7e3x3HglpASbxLAVRN1iu0wl3ZRLgrmWdxFW9Fi163sE0qZxwaYPbzzwwlyHqxhz-dKceccgh6spSt~4t2AH2AuyMJY4D0o3QI-z5CP6BBLY7nNdNfwdAovbEYtcBs9I44Rs1mf44WNrW0Uaxres8gMxJWugPbqPuj~RPt58zDLZPa2pw40HFaXx7amyNDv3RS3suaAqer8v17MkeNX1Am1HhF7jCPONQvIRFJnRakYW~fhRxA3AGF4UwL-KFvs0Ki5tIxAOexFrjXNMXwcbKw2VMcEtFHs8lruAQj9GvrHoMZkTe6z9KFTQ__&Key-Pair-Id=APKAINTVSUGEWH5XD5UA",
                        LastRevised = DateTimeOffset.UtcNow
                    },
                    new Summary
                    {
                        Content = "# Discord Developer Stage - January 2022\r\nThis is the description text underneath.\r\n\r\n## What\'s new\r\n- blah\r\n  - bulletpoint\r\n  > text\r\n  [link](https://discord.com/channels/936725123724111953/937130072551346216/937804751289081866)\r\n![image](https://media.discordapp.net/attachments/937130072551346216/937804320710230037/unknown.png?width=1309&height=903)",
                        EventId = id,
                        IsNew = true,
                        Status = FeatureType.PlannedQ2,
                        SummaryId = Guid.NewGuid(),
                        Title = "Partial markdown test",
                        Type = SummaryType.Feature,
                        Url = "https://s3-alpha-sig.figma.com/img/2df7/f218/98835dfa567742829fbd6558f88fd217?Expires=1644796800&Signature=PO8t2XjqNMhPb-7SFw0OptWjCMCTMY7e3x3HglpASbxLAVRN1iu0wl3ZRLgrmWdxFW9Fi163sE0qZxwaYPbzzwwlyHqxhz-dKceccgh6spSt~4t2AH2AuyMJY4D0o3QI-z5CP6BBLY7nNdNfwdAovbEYtcBs9I44Rs1mf44WNrW0Uaxres8gMxJWugPbqPuj~RPt58zDLZPa2pw40HFaXx7amyNDv3RS3suaAqer8v17MkeNX1Am1HhF7jCPONQvIRFJnRakYW~fhRxA3AGF4UwL-KFvs0Ki5tIxAOexFrjXNMXwcbKw2VMcEtFHs8lruAQj9GvrHoMZkTe6z9KFTQ__&Key-Pair-Id=APKAINTVSUGEWH5XD5UA",
                        LastRevised = DateTimeOffset.UtcNow
                    }
                },
                Thumbnail = "https://s3-alpha-sig.figma.com/img/29be/1a71/0486e49300b467e39356856a8d45c5b7?Expires=1644796800&Signature=LSOgUDrouk7emA8gV2H06QP~nhnu~Lw3JMrw~MzkyHn3LqG2CxgoKH8~COuiNYT0hbD~4OfSGoy9P~8JqUSd09ePq0BZYCgBz9m~Ee8i8XQ8SllqnkrKhHWSWyvgRFVhPPqRrQsz8dRvDLFn-AVBgNT1G7ETlwoscHzibVS7R2z5oaEl6ZIWS35Fx8-ZKmZtEWBqKDnBe0QUKhIxH7yPrvPe~nf9SGXpAXcymUXbYedhMO2pDGLyV4u8bI02jXlJf2PNNyK8bVJ3B3ycSMlaTijs0xQahyOq1I2Y0VkFQ5-MJ6wiutPaA~4ZJkJ731BMI21WI5sBe75AB5ah4cxG7A__&Key-Pair-Id=APKAINTVSUGEWH5XD5UA",
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