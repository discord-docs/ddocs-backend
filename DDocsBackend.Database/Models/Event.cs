using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    /// <summary>
    ///     An object representing a DDevs event.
    /// </summary>
    public class Event
    {
        /// <summary>
        ///     Gets or sets the unique id for this event.
        /// </summary>
        [Key]
        public Guid EventId { get; set; }

        /// <summary>
        ///     Gets or sets the time the event started.
        /// </summary>
        public DateTimeOffset HeldAt { get; set; }

        /// <summary>
        ///     Gets or sets the optional name of the event. This field defaults to "Discord Developers {month} stage."
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        ///     Gets or sets the optional description of the event.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        ///     Gets or sets the authors that contributed to this events summary.
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
    }
}
