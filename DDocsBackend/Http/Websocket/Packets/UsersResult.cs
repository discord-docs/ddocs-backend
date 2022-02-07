using DDocsBackend.RestModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    internal class UsersResult
    {
        [JsonProperty("users")]
        public Author[]? Users { get; set; }
    }
}
