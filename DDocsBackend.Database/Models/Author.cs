using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    /// <summary>
    ///     Represents the author of an event post on the website.
    /// </summary>
    public partial class Author
    {
        public Author()
        {

        }

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the snowflake userid of the author.
        /// </summary>
        public ulong UserId { get; set; }

        public virtual ICollection<Event>? Events { get; set; }    

        /// <summary>
        ///     Gets or sets whether their existence in an Event object was to "modify" or revise it.
        /// </summary>
        public bool Revised { get; set; }
    }
}
