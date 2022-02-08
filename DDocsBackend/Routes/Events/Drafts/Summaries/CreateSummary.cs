using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events.Drafts.Summaries
{
    internal class CreateSummary : RestModuleBase
    {
        [Route(@"\/drafts\/(.{32})\/summaries$", "POST", true)]
        [RequireAuthentication(true)]
        public async Task<RestResult> ExecuteAsync(string draftId)
        {
            if (!Guid.TryParse(draftId, out var id))
                return RestResult.BadRequest;

            var summaryBody = GetBody<CreateSummaryBody>();

            if (summaryBody == null)
                return RestResult.BadRequest;

            var draft = await DataAccessLayer.ModifyDraftAsync(id, x =>
            {
                x.Summaries.Add(new Data.Models.Summary
                {
                    Title = summaryBody.Title,
                    Content = summaryBody.Content,
                    IsNew = summaryBody.IsNew,
                    LastRevised = DateTimeOffset.UtcNow,
                    Status = summaryBody.FeatureType.ToNullable(),
                    Type = summaryBody.Type
                });

                if (x.Author!.UserId != Authentication!.UserId && !x.Contributors.Any(y => y.UserId == Authentication!.UserId))
                {
                    x.Contributors.Add(new Data.Models.Author
                    {
                        UserId = Authentication!.UserId,
                        Revised = true,
                    });
                }
            }).ConfigureAwait(false);

            if (draft == null)
                return RestResult.NotFound;

            var model = await draft.ToRestModelAsync(this).ConfigureAwait(false);

            WebsocketServer.DispatchEvent(EventTypes.DraftModified, model);

            return RestResult.OK.WithData(model);
        }
    }
}
