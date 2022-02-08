using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events
{
    public class GetEvent : RestModuleBase
    {
        [Route("/events/{eventId}", "GET")]
        public async Task<RestResult> GetEventAsync(string eventId)
        {
            if(!Guid.TryParse(eventId, out var guid))
            {
                return RestResult.BadRequest;
            }

            var evnt = await DataAccessLayer.GetEventAsync(guid).ConfigureAwait(false);

            if (evnt == null)
                return RestResult.NotFound;

            var author = evnt.Authors!.FirstOrDefault(x => !x.Revised);

            return RestResult.OK.WithData(new Event
            {
                Author = await author!.ToRestModelAsync(this),
                // bit of a sus async bypass here
                Contributors = await Task.WhenAll(evnt.Authors?.Where(x => x.Revised).Select(x => x.ToRestModelAsync(this))!),
                Description = evnt.Description,
                EventId = evnt.EventId.ToString("N"),
                HeldAt = evnt.HeldAt,
                Summaries = evnt.Summaries!.Select(x => x.ToRestModel()),
                Title = evnt.Title,
                Thumbnail = evnt.Thumbnail,
                LastRevised = (evnt.Summaries?.Any() ?? false) ? evnt.Summaries!.Max(x => x.LastRevised) : evnt.HeldAt
            });
        }
    }
}
