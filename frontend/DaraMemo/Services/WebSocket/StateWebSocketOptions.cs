using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Services.WebSocket {

    public sealed class StateWebSocketOptions {
        public required string Host { get; init; }  // 例: "localhost:5000"
    }

}
