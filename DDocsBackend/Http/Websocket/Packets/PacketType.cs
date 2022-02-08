using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    public enum PacketType
    {
        Identify,
        Hello,
        Heartbeat,
        HeartbeatAck,
        SwitchingPage,
        Event,
        GetUsers,
        Users,
        UpdateIntents,
    }
}
