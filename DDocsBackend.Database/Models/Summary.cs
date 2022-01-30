using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    /// <summary>
    ///     An object representing information summarized from an event.
    /// </summary>
    public class Summary
    {
        /// <summary>
        ///     Gets or sets the unique identifier for this summary.
        /// </summary>
        [Key]
        public Guid SummaryId { get; set; }

        /// <summary>
        ///     Gets or sets the title of the summary.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        ///     Gets or sets the type of this summary.
        /// </summary>
        public SummaryType Type { get; set; }

        /// <summary>
        ///     Gets or sets whether this summary involves a feature being new or not.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        ///     Gets or sets the optional thumbnail.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        ///     Gets or sets the markdown content of this summary.
        /// </summary>
        public string? Content { get; set; }
    }
}
