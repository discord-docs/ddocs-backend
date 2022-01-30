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

            Response.SetCookie(new System.Net.Cookie("r_", result.Authentication.JWTRefreshToken)
            {
                Expires = DateTime.UtcNow.AddDays(7),
#if DEBUG == false
                Domain = "ddocs.io",
                Secure = true
#endif
            });

            return RestResult.OK.WithData(new
            {
                token = $"{result.Token}"
            });
        }
    }
}
