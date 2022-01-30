using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Context
{
    /// <summary>
    ///     Represents a factory for creating <see cref="DDocsContext"/>.
    /// </summary>
    public class DDocsContextFactory : IDesignTimeDbContextFactory<DDocsContext>
    {
        /// <inheritdoc/>
        public DDocsContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder()
                .UseNpgsql(configuration["CONNECTION_STRING"]);

            return new DDocsContext(optionsBuilder.Options);
        }
    }
}
