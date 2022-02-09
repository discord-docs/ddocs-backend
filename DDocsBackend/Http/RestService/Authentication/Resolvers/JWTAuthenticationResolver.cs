using DDocsBackend.Data;
using DDocsBackend.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http
{
    internal class JWTAuthenticationResolver : IAuthenticationResolver
    {
        public string Prefix
            => "Bearer";

        public async Task<IAuthentication?> ExecuteAsync(string auth, IServiceProvider provider)
        {
            var authService = provider.GetRequiredService<AuthenticationService>();
            var dataAccessLayer = provider.GetRequiredService<DataAccessLayer>();

            var result = await authService.GetAuthenticationAsync(auth).ConfigureAwait(false);

            if (result == null)
                return null;

            return new JWTAuthentication(result,
                await dataAccessLayer.IsAuthorAsync(result.UserId).ConfigureAwait(false),
                await dataAccessLayer.IsAdminAsync(result.UserId).ConfigureAwait(false));
        }
    }
}
