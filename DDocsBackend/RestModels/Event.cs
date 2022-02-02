using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    public class Event : PartialEvent
    {
        [JsonProperty("author")]
        public Author? Author { get; set; }

        [JsonProperty("contributors")]
        public IEnumerable<Author?> Contributors { get; set; } = new List<Author>();

        [JsonProperty("summaries")]
        public IEnumerable<EventSummary?> Summaries { get; set; } = new List<EventSummary>();

        [JsonProperty("lastRevised")]
        public DateTimeOffset LastRevised { get; set; }

    }
}
