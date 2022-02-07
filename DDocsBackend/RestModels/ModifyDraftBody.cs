using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    public class ModifyDraftBody
    {
        [JsonProperty("heldAt")]
        public Optional<DateTimeOffset> HeldAt { get; set; }

        [JsonProperty("title")]
        public Optional<string?> Title { get; set; }

        [JsonProperty("description")]
        public Optional<string?> Description { get; set; }

        [JsonProperty("thumbnail")]
        public Optional<string?> Thumbnail { get; set; }
    }
}
