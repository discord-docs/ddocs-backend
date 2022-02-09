using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http
{
    public class JWTAuthentication : IAuthentication
    {
        public ulong UserId { get; set; }

        public string? JWTRefreshToken { get; set; }

        public DateTimeOffset RefreshExpiresAt { get; set; }

        public bool IsAuthor { get; set; }

        public bool IsAdmin { get; set; }

        public JWTAuthentication(Data.Models.Authentication jwtData, bool isAuthor, bool isAdmin)
        {
            UserId = jwtData.UserId;
            JWTRefreshToken= jwtData.JWTRefreshToken;
            RefreshExpiresAt = jwtData.RefreshExpiresAt;
            IsAdmin = isAdmin;
            IsAuthor = isAuthor;
        }
    }
}
