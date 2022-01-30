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

        /// <summary>
        ///     Initializes a new instance of the <see cref="DDocsContext"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/> to be injected.</param>
        public DDocsContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // todo
        }
    }
}
