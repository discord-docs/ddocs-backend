using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend
{
    public class RestModuleBase
    {
        public HttpListenerContext? Context { get; private set; }

        public HttpServer? RestServer { get; private set; }

        public HttpListenerRequest? Request
           => Context?.Request;
        public HttpListenerResponse? Response
            => Context?.Response;

        internal RestModuleInfo? ModuleInfo { get; private set; }

        internal RestModuleBase InitializeModule(HttpListenerContext context, RestModuleInfo info, HttpServer server)
        {
            this.Context = context;
            this.ModuleInfo = info;
            this.RestServer = server;
            return this;
        }

        public override bool Equals(object? obj)
        {
            try
            {
                if (obj == null)
                    return false;

                if (obj is RestModuleBase other)
                {
                    return other.ModuleInfo?.Equals(this.ModuleInfo) ?? false;
                }
                else return base.Equals(obj);
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
