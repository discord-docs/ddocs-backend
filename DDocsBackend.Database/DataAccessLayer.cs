using DDocsBackend.Data.Context;
using Microsoft.EntityFrameworkCore;

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
    }
}