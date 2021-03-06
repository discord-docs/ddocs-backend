using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    public class User
    {
        [JsonProperty("uid")]
        public ulong Id { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("discriminator")]
        public string? Discriminator { get; set; }

        [JsonProperty("avatar")]
        public string? Avatar { get; set; }

        [JsonProperty("isAuthor")]
        public bool IsAuthor { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }
    }
}
