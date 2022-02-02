using DDocsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<VerifiedAuthors> VerifiedAuthors { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DDocsContext"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/> to be injected.</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DDocsContext(DbContextOptions options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("fuzzystrmatch");
        }
    }
}
