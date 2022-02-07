using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    internal class PageSwitch
    {
        [JsonProperty("page")]
        public string? Page { get; set; }
    }
}
