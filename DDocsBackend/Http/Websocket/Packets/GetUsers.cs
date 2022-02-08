using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    public class GetUsers
    {
        [JsonProperty("all")]
        public Optional<bool> All { get; set; }
    }
}
