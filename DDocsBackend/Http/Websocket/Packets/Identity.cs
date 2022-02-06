using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    public class Identity
    {
        [JsonProperty("token")]
        public string? Token { get; set; }

        [JsonProperty("events")]
        public EventTypes Events { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(Token);
        }
    }
}
