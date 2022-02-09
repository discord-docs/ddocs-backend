using DDocsBackend.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http
{
    internal class AdministatorAuthenticationResolver : IAuthenticationResolver
    {
        public string Prefix
            => "Admin";

        public async Task<IAuthentication?> ExecuteAsync(string auth, IServiceProvider provider)
        {
            string? hash;
            using (var sha = SHA512.Create())
            {
                hash = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(auth))).Replace("-", "");
            }

            var dataAccessLayer = provider.GetRequiredService<DataAccessLayer>();

            var admin = await dataAccessLayer.GetAdminAsync(hash).ConfigureAwait(false);

            if (admin == null)
                return null;

            return new AdministatorAuthentication(admin, await dataAccessLayer.IsAuthorAsync(admin.UserId).ConfigureAwait(false));
        }
    }
}
