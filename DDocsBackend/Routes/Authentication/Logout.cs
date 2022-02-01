using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Authentication
{
    public class Logout : RestModuleBase
    {
        [Route("/auth/logout", "POST")]
        [RequireAuthentication]
        public async Task<RestResult> ExecuteAsync()
        {
            await DataAccessLayer.DeleteAuthenticationAsync(Authentication!).ConfigureAwait(false);
            ClearRefreshCookie();
            return RestResult.OK;
        }
    }
}
