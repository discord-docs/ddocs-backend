using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events.Drafts.Summaries
{
    internal class DeleteSummary : RestModuleBase
    {
        [Route(@"\/drafts\/(.{32})\/summaries\/(.{32})$", "DELETE", true)]
        [RequireAuthentication(true)]
        public async Task<RestResult> ExecuteAsync(string rawDraftId, string rawSummaryId)
        {
            if (!Guid.TryParse(rawDraftId, out var draftId) || !Guid.TryParse(rawSummaryId, out var summaryId))
                return RestResult.BadRequest;

            bool summaryFound = false;
            var draft = await DataAccessLayer.ModifyDraftAsync(draftId, x =>
            {
                var sum = x.Summaries.FirstOrDefault(x => x.SummaryId == summaryId);

                if(sum != null)
                {
                    x.Summaries.Remove(sum);
                    summaryFound = true;
                }
            }).ConfigureAwait(false);

            if (draft == null || !summaryFound)
                return RestResult.NotFound;

            return RestResult.OK.WithData(await draft.ToRestModelAsync(this).ConfigureAwait(false));
        }
    }
}
