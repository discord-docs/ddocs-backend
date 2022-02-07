using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    public class EventDraft
    {
        [Key]
        public Guid DraftId { get; set; }
        public string? Title { get; set; }
        public Author? Author { get; set; }
        public ICollection<Author> Contributors { get; set; } = new List<Author>();
        public DateTimeOffset HeldAt { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public ICollection<Summary> Summaries { get; set; } = new List<Summary>();
    }
}
