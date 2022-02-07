using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    public class Packet
    {
        [JsonProperty("op")]
        public PacketType Type { get; set; }

        [JsonProperty("p")]
        public object? Payload { get; set; }

        public T? PayloadAs<T>() where T : class
            => (Payload as JObject)?.ToObject<T>();
    }
}
