using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    public class Authentication
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong UserId { get; set; }

        public string? JWTRefreshToken { get; set; }

        public DateTimeOffset RefreshExpiresAt { get; set; }
    }
}
