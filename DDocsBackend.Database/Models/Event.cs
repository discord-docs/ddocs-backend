using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    /// <summary>
    ///     An object representing a DDevs event.
    /// </summary>
    internal class Event
    {
        /// <summary>
        ///     Gets or sets the time the event started.
        /// </summary>
        public DateTimeOffset HeldAt { get; set; }

        /// <summary>
        ///     Gets or sets the authors that contributed to this events summary/
        /// </summary>
        public List<Author>? Authors { get; set; }

        /// <summary>
        ///     Gets or sets the summaries for this event.
        /// </summary>
        public List<Summary>? Summaries { get; set; }

        /// <summary>
        ///     Gets or sets whether or not this event is outdated.
        /// </summary>
        public bool Deprecated { get; set; } = false;

        public List<Summary>? RelatedSummaries { get; set; };
    }
}
