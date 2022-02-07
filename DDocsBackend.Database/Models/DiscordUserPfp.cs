using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    public class DiscordUserPfp
    {
        [Key]
        public ulong UserId { get; set; }
        public string? AssetId { get; set; }
        public virtual Asset? Asset { get; set; }
    }
}
