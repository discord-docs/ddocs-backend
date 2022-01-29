using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes
{
    public class Health : RestModuleBase
    {
        [Route("/health", "GET")]
        public Task<RestResult> ExecuteHealthAsync()
        {
            return Task.FromResult(RestResult.OK);
        }
    }
}
