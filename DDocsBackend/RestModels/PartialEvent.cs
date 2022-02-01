using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    public class PartialEvent
    {
        [JsonProperty("id")]
        public string? EventId { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonProperty("heldAt")]
        public DateTimeOffset HeldAt { get; set; }
    }
}
