using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Authentication
{
    public class RefreshToken : RestModuleBase
    {
        private Logger _log
            => Logger.GetLogger<RefreshToken>();

        [Route("/auth/refresh", "GET")]
        public async Task<RestResult> ExecuteAsync()
        {   
            var refreshToken = Request.Cookies["r_"];

            if (refreshToken == null)
                return RestResult.Unauthorized;

            var result = await AuthenticationService.ApplyRefreshTokenAsync(refreshToken.Value);

            if (!result.HasValue)
            {
                _log.Warn("Got no value on applying refresh token", Severity.Rest);
                return RestResult.BadRequest;
            }

            SetRefreshCookie(result.Value.newRefresh);

            return RestResult.OK.WithData(new
            {
                token = $"{result.Value.newJWT}"
            });
        }
    }
}
