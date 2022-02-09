using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DDocsBackend.Http
{
    internal class AuthenticationResolver
    {
        private static Regex _authenticationRegex = new(@"^(.+) (.+)$");
        private static List<IAuthenticationResolver> _resolvers = new()
        {
            new JWTAuthenticationResolver(),
            new AdministatorAuthenticationResolver()
        };

        public static async Task<IAuthentication?> ResolveAsync(string auth, IServiceProvider provider)
        {
            var result = _authenticationRegex.Match(auth);

            if (!result.Success)
                return null;

            var resolver = _resolvers.FirstOrDefault(x => x.Prefix == result.Groups[1].Value);

            if (resolver == null)
                return null;

            return await resolver.ExecuteAsync(result.Groups[2].Value, provider).ConfigureAwait(false);
        }
    }
}
