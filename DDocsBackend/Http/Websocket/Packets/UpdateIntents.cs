using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    public class UpdateIntents
    {
        [JsonProperty("intents")]
        public EventTypes Types { get; set; }
    }
}
