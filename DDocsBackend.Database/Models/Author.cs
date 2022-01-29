using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    /// <summary>
    ///     Represents the author of an event post on the website.
    /// </summary>
    internal class Author
    {
        /// <summary>
        ///     Gets or sets the name of the author in the format <c>username#discriminator</c>.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     Gets or sets the snowflake userid of the author.
        /// </summary>
        public ulong UserId { get; set; }

        /// <summary>
        ///     Gets or sets whether their existence in an Event object was to "modify" or revise it.
        /// </summary>
        public bool Revised { get; set; }
    }
}
