using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events.Drafts
{
    internal class CreateDraft : RestModuleBase
    {
        [Route("/drafts", "POST")]
        [RequireAuthentication(true)]
        public async Task<RestResult> ExecuteAsync()
        {
            var body = GetBody<CreateDraftBody>();

            if (body == null)
                return RestResult.BadRequest;

            if (!body.ValidateBody())
                return RestResult.BadRequest;

            var draft = await DataAccessLayer.CreateDraftAsync(body.Title!, Authentication!.UserId, body.HeldAt, body.Description, body.Thumbnail);

            var model = await draft.ToRestModelAsync(this).ConfigureAwait(false);

            WebsocketServer.DispatchEvent(EventTypes.DraftCreated, model);

            return RestResult.OK.WithData(model);
        }
    }
}
