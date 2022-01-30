using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Authentication
{
    public class OAuthCallback : RestModuleBase
    {
        [Route("/auth/login?q={code}", "GET")]
        public Task<RestResult> ExecuteAsync(string code)
        {
            return Task.FromResult(RestResult.OK);
        }
    }
}
