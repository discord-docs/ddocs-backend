using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http
{
    public interface IAuthenticationResolver 
    {
        string Prefix { get; }

        Task<IAuthentication?> ExecuteAsync(string auth, IServiceProvider provider);
    }
}
