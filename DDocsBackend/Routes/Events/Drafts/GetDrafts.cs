using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events.Drafts
{
    public class GetDrafts :RestModuleBase
    {
        [Route("/drafts", "GET")]
        [RequireAuthentication(true)]
        public async Task<RestResult> ExecuteAsync()
        {
            return RestResult.OK.WithData(await Task.WhenAll((await DataAccessLayer.GetDraftsAsync()).Select(x => x.ToRestModelAsync(this))));
        }
    }
}
