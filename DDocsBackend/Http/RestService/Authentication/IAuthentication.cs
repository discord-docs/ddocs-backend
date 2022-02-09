using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http
{
    public interface IAuthentication
    {
        ulong UserId { get; set; }

        bool IsAuthor { get; set; }

        bool IsAdmin { get; set; }
    }
}
