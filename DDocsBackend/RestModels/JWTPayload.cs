using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.RestModels
{
    internal class JWTPayload
    {
        [JsonIgnore]
        public DateTimeOffset IssuedAt
        {
            get => DateTimeOffset.FromUnixTimeSeconds(RawIssuedAt);
            set
            {
                RawIssuedAt = value.ToUnixTimeSeconds();
            }
        }

        [JsonIgnore]
        public DateTimeOffset ExpiresAt
        {
            get => DateTimeOffset.FromUnixTimeSeconds(RawExpiresAt);
            set
            {
                RawExpiresAt = value.ToUnixTimeSeconds();
            }
        }

        [JsonProperty("user_id")]
        public ulong UserId { get; set; }

        [JsonProperty("iat")]
        public long RawIssuedAt { get; set; }

        [JsonProperty("exp")]
        public long RawExpiresAt { get; set; }
    }
}
