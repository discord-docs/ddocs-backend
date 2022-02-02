using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events
{
    public class SearchEvents : RestModuleBase
    {
        [Route("/events?search={query}", "GET")]
        public async Task<RestResult> ExecuteAsync(string query)
        {
            var result = await DataAccessLayer.SearchEventsAsync(query).ConfigureAwait(false);

            return RestResult.OK.WithData(result.Select(x => new PartialEvent
            {
                Description = x.Description,
                EventId = x.EventId.ToString("N"),
                Thumbnail = x.Thumbnail,
                Title = x.Title
            }));
        }
    }
}
