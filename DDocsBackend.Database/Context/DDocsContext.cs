using DDocsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Context
{
    /// <summary>
    ///     Implementation of <see cref="DbContext"/> for DDocs
    /// </summary>
    public class DDocsContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Summary> Summaries { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Authentication> Authentication { get; set; }
        public DbSet<DiscordOAuthAuthentication> DiscordAuthentication { get; set; }
        public DbSet<VerifiedAuthor> VerifiedAuthors { get; set; }
        public DbSet<EventDraft> Drafts { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<DiscordUserPfp> UserProfiles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SiteContributor> SiteContributors { get; set; }

        private readonly Logger _log;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DDocsContext"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/> to be injected.</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DDocsContext(DbContextOptions options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
            _log = Logger.GetLogger<DDocsContext>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiscordUserPfp>()
                .HasOne<Asset>(s => s.Asset)
                .WithOne()
                .HasForeignKey<DiscordUserPfp>(ad => ad.AssetId);

            modelBuilder.Entity<SiteContributor>()
                .HasOne<Asset>(s => s.ProfilePicture)
                .WithOne()
                .HasForeignKey<SiteContributor>(x => x.ProfilePictureId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo((a, b) => b switch
            {
                LogLevel.Trace or 
                LogLevel.Information or 
                LogLevel.Critical or
                LogLevel.Warning or 
                LogLevel.Error => true,
                _ => false
            }, (data) =>
            {
                var sevs = new Severity[]
                {
                    Severity.Database,
                    data.LogLevel switch
                    {
                        LogLevel.Debug => Severity.Debug,
                        LogLevel.Critical => Severity.Critical,
                        LogLevel.Error => Severity.Error,
                        LogLevel.Information => Severity.Info,
                        LogLevel.None => Severity.Log,
                        LogLevel.Trace => Severity.Trace,
                        LogLevel.Warning => Severity.Warning,
                        _ => Severity.Log,
                    }
                };
                _log.Write(data.ToString(), sevs);
            });
        }
    }
}
