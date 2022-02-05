using DDocsBackend.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    internal class ModifySummaryBody
    {
        [JsonProperty("title")]
        public Optional<string?> Title { get; set; }

        [JsonProperty("content")]
        public Optional<string?> Content { get; set; }

        [JsonProperty("type")]
        public Optional<SummaryType> Type { get; set; }

        [JsonProperty("isNew")]
        public Optional<bool> IsNew { get; set; }

        [JsonProperty("featureType")]
        public Optional<FeatureType?> FeatureType { get; set; }
    }
}
