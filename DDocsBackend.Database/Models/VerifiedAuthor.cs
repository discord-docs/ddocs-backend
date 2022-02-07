using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    public class VerifiedAuthor
    {
        [Key]
        public ulong UserId { get; set; }

        public string? Username { get; set; }

        public string? Discriminator { get; set; }

        public string? AvatarId { get; set; }
    }
}
