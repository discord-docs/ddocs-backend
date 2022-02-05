using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events.Drafts
{
    public class GetDraft : RestModuleBase
    {
        [Route(@"\/drafts\/(.{32})$", "GET", true)]
        [RequireAuthentication(true)]
        public async Task<RestResult> ExecuteAsync(string draftId)
        {
            if(!Guid.TryParse(draftId, out var id))
                return RestResult.BadRequest;

            var draft = await DataAccessLayer.GetDraftAsync(id).ConfigureAwait(false);

            if(draft == null)
                return RestResult.NotFound;

            return RestResult.OK.WithData(await draft.ToRestModelAsync(this).ConfigureAwait(false));
        }
    }
}
