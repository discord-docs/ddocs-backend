using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events
{
    public class GetEvents : RestModuleBase
    {
        [Route("/events?year={year}", "GET")]
        public async Task<RestResult> GetEventsAsync(int year)
        {
            var events = await DataAccessLayer.GetEventsAsync(year).ConfigureAwait(false);

            return RestResult.OK.WithData(events.Select(x => new PartialEvent
            {
                Description = x.Description,
                EventId = x.EventId.ToString("N"),
                Thumbnail = x.Thumbnail,
                Title = x.Title
            }));
        }
    }
}
