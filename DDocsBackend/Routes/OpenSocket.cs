﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes
{
    public class OpenSocket : RestModuleBase
    {
        [Route("/socket", "GET")]
        [RequireAuthentication]
        public async Task<RestResult> ExecuteAsync()
        {
            if (!Request.IsWebSocketRequest)
                return RestResult.BadRequest;

            var result = await WebsocketServer.TryAcceptSocketAsync(Context).ConfigureAwait(false);

            if (!result)
                return RestResult.BadRequest;

            return RestResult.NoAction;
        }
    }
}
