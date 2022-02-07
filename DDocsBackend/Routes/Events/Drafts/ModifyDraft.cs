using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events.Drafts
{
    internal class ModifyDraft : RestModuleBase
    {
        [Route(@"\/drafts\/(.{32})$", "PATCH", true)]
        [RequireAuthentication(true)]
        public async Task<RestResult> ExecuteAsync(string draftId)
        {
            if (!Guid.TryParse(draftId, out var id))
                return RestResult.BadRequest;

            var body = GetBody<ModifyDraftBody>();

            if (body == null)
                return RestResult.BadRequest;

            var draft = await DataAccessLayer.ModifyDraftAsync(id, x =>
            {
                if (body.Description.IsSpecified)
                    x.Description = body.Description.Value;

                if (body.HeldAt.IsSpecified)
                    x.HeldAt = body.HeldAt.Value;

                if(body.Thumbnail.IsSpecified)
                    x.Thumbnail = body.Thumbnail.Value;

                if (body.Title.IsSpecified)
                    x.Title = body.Title.Value;

                if(x.Author!.UserId != Authentication!.UserId && !x.Contributors.Any(y => y.UserId == Authentication!.UserId))
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

            return RestResult.OK.WithData(await draft.ToRestModelAsync(this).ConfigureAwait(false));
        }
    }
}
