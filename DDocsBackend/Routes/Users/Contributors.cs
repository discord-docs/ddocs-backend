using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Users
{
    public class Contributors : RestModuleBase
    {
        [Route("/users/admin", "GET")]
        [RequireAdmin]
        public Task<RestResult> ExecuteAsync()
        {
            return Task.FromResult(RestResult.OK);
        }
    }
}
