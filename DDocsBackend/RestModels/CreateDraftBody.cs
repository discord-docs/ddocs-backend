using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    public class CreateDraftBody
    {
        [JsonProperty("heldAt")]
        public DateTimeOffset HeldAt { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }

        public virtual bool ValidateBody()
        {
            if (
                string.IsNullOrEmpty(Title) ||
                Title.Length > 256 ||
                Description?.Length > 4096 ||
                HeldAt == default
                )
                return false;

            return true;
        }
    }
}
