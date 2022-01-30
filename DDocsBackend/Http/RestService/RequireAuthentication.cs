using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend
{
    /// <summary>
    ///     Specifies this route requires JWT authentication.
    /// </summary>
    /// <remarks>
    ///     The function wont execute without the valid authentication.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireAuthentication : Attribute { }
}
