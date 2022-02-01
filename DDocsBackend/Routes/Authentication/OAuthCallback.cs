using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Authentication
{
    public class OAuthCallback : RestModuleBase
    {
        [Route("/auth/login?code={code}", "GET")]
        public async Task<RestResult> ExecuteAsync(string code)
        {
            var tokenResult = await DiscordOAuthHelper.GetTokenAsync(code);

            if (tokenResult == null)
                return RestResult.BadRequest;

            var result = await AuthenticationService.CreateAuthenticationAsync(tokenResult);

            SetRefreshCookie(result.Authentication.JWTRefreshToken!);

            return RestResult.OK.WithData(new
            {
                token = $"{result.Token}"
            });
        }
    }
}
