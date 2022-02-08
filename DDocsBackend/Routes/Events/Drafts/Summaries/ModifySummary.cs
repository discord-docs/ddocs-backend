using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events.Drafts.Summaries
{
    internal class ModifySummary : RestModuleBase
    {
        [Route(@"\/drafts\/(.{32})\/summaries\/(.{32})$", "PATCH", true)]
        [RequireAuthentication(true)]
        public async Task<RestResult> ExecuteAsync(string rawDraftId, string rawSummaryId)
        {
            if (!Guid.TryParse(rawDraftId, out var draftId) || !Guid.TryParse(rawSummaryId, out var summaryId))
                return RestResult.BadRequest;

            var body = GetBody<ModifySummaryBody>();

            if (body == null)
                return RestResult.NotFound;

            bool summaryFound = false;

            var draft = await DataAccessLayer.ModifyDraftAsync(draftId, x =>
            {
                var sum = x.Summaries.FirstOrDefault(y => y.SummaryId == summaryId);
                
                if(sum != null)
                {
                    if (body.Content.IsSpecified)
                        sum.Content = body.Content.Value;

                    if (body.Title.IsSpecified)
                        sum.Title = body.Title.Value;

                    if (body.FeatureType.IsSpecified)
                        sum.Status = body.FeatureType.Value;

                    if (body.IsNew.IsSpecified)
                        sum.IsNew = body.IsNew.Value;

                    if (body.Type.IsSpecified)
                        sum.Type = body.Type.Value;

                    if (x.Author!.UserId != Authentication!.UserId && !x.Contributors.Any(y => y.UserId == Authentication!.UserId))
                    {
                        x.Contributors.Add(new Data.Models.Author
                        {
                            UserId = Authentication!.UserId,
                            Revised = true,
                        });
                    }

                    summaryFound = true;
                }
            }).ConfigureAwait(false);

            if (draft == null || !summaryFound)
                return RestResult.NotFound;

            var model = await draft.ToRestModelAsync(this).ConfigureAwait(false);

            WebsocketServer.DispatchEvent(EventTypes.DraftModified, model);

            return RestResult.OK.WithData(model);
        }
    }
}
