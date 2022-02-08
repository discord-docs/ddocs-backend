using DDocsBackend.RestModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    internal class ExtendedAuthor : Author
    {
        [JsonProperty("page")]
        public Optional<string?> Page { get; set; }

        public ExtendedAuthor() { }

        public ExtendedAuthor(Author author)
        {
            base.Avatar = author.Avatar;
            base.Discriminator = author.Discriminator;
            base.UserId = author.UserId;
            base.Username = author.Username;
        }

        public ExtendedAuthor(string? page, Author author)
            : this(author)
        {
            Page = page;
        }
    }
}
