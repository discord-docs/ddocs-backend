using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    public class Draft
    {
        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("heldAt")]
        public DateTimeOffset HeldAt { get; set; }

        [JsonProperty("id")]
        public Guid DraftId { get; set; }

        [JsonProperty("author")]
        public Author? Author { get; set; }

        [JsonProperty("contributors")]
        public List<Author> Contributors { get; set; } = new();

        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonProperty("summaries")]
        public List<EventSummary> Summaries { get; set; } = new();
    }
}
