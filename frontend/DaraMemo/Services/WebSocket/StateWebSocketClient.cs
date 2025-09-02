using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;

namespace DaraMemo.Services.WebSocket {
    public class StateWebSocketClient(string host) {
        private readonly Uri _uri = new($"ws://{host}/ws/state");
        private readonly ClientWebSocket _webSocket = new();
        private CancellationTokenSource _cts = new();

        public event Action<string, DateTime> OnStateChanged = delegate { };
        public event Action<string, DateTime> OnTicked = delegate { };

        public async Task ConnectAsync() {
            _cts = new CancellationTokenSource();
            await _webSocket.ConnectAsync(_uri, _cts.Token);
            _ = ReceiveLoopAsync(_cts.Token);
        }

        public async Task DisconnectAsync() {
            _cts.Cancel();
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }

        private async Task ReceiveLoopAsync(CancellationToken cancellationToken) {
            var buffer = new byte[4096];

            while (_webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested) {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close) {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", cancellationToken);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                HandleMessage(message);
            }
        }

        private void HandleMessage(string json) {
            try {
                var msg = JsonConvert.DeserializeObject<StateMessage>(json);
                if (msg == null) return;

                var status = msg.Status.ToUpperInvariant();
                var time = msg.ServerTime;

                switch (msg.Type.ToLowerInvariant()) {
                    case "changed":
                        OnStateChanged?.Invoke(status, time);
                        break;
                    case "ticked":
                        OnTicked?.Invoke(status, time);
                        break;
                }
            } catch (Exception ex) {
                Console.WriteLine($"WebSocket message handling error: {ex.Message}");
            }
        }

        private class StateMessage {
            [JsonProperty("type")]
            public string Type { get; set; } = string.Empty;

            [JsonProperty("status")]
            public string Status { get; set; } = string.Empty;

            [JsonProperty("server_time")]
            public DateTime ServerTime { get; set; }
        }
    }
}

