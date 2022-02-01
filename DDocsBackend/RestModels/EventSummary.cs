using DDocsBackend.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    public class EventSummary
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }

        [JsonProperty("type")]
        public SummaryType Type { get; set; }

        [JsonProperty("isNew")]
        public bool IsNew { get; set; }

        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonProperty("featureType")]
        public FeatureType? FeatureType { get; set; }
    }
}
