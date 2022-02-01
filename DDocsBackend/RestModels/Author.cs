using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    public class Author
    {
        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("discriminator")]
        public string? Discriminator { get; set; }

        [JsonProperty("id")]
        public ulong UserId { get; set; }

        [JsonProperty("avatar")]
        public string? Avatar { get; set; }
    }
}
