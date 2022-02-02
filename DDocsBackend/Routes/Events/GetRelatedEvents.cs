using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events
{
    public class GetRelatedEvents : RestModuleBase
    {
        [Route("/events/{eventId}/related", "GET")]
        public async Task<RestResult> ExecuteAsync(string eventId)
        {
            if(!Guid.TryParse(eventId, out var id))
            {
                return RestResult.BadRequest;
            }

            var evnt = await DataAccessLayer.GetEventAsync(id);

            if(evnt == null)
            {
                return RestResult.NotFound;
            }    

            // get "related" events, in the future this will be off of tags and such...
            var related = await DataAccessLayer.GetEventsAsync(evnt.HeldAt.Year);

            return RestResult.OK.WithData(related.Select(x => new PartialEvent
            {
                Description = x.Description,
                EventId = x.EventId.ToString("N"),
                Thumbnail = x.Thumbnail,
                Title = x.Title
            }));
        }
    }
}
