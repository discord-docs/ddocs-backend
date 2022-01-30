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
    }
}