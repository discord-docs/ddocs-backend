using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    public class SiteContributor
    {
        [Key]
        public ulong UserId { get; set; }
        public string? Username { get; set; }
        public string? Discriminator { get; set; }
        public Asset? ProfilePicture { get; set; }
        public string? ProfilePictureId { get; set; }
    }
}
