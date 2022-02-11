using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events
{
    public class CreateEvent : RestModuleBase
    {
        [Route("/events", "POST")]
        [RequireAdmin]
        public async Task<RestResult> CreateEventAsync()
        {
            await DataAccessLayer.CreateEventAsync();
            await DataAccessLayer.CreateEventAsync();
            await DataAccessLayer.CreateEventAsync();
            await DataAccessLayer.CreateEventAsync();

            return RestResult.OK;
        }
    }
}
