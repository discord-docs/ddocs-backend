using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http
{
    internal class AdministatorAuthentication : IAuthentication
    {
        public ulong UserId { get; set; }
        public bool IsAuthor { get; set; }
        public bool IsAdmin { get; set; }

        public AdministatorAuthentication() { }

        public AdministatorAuthentication(Data.Models.Admin admin, bool isAuthor)
        {
            UserId = admin.UserId;
            IsAuthor = isAuthor;
            IsAdmin = true;
        }
    }
}
