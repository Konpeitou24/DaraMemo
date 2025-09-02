using DaraMemo.Services.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Services.WebSocket {
    /// <summary>
    /// アプリ起動時に StateWebSocketClient を接続し、終了時に切断する HostedService。
    /// </summary>
    public sealed class StateWebSocketService : IHostedService {
        private readonly StateWebSocketClient _client;
        private readonly ILogger<StateWebSocketService> _logger;

        public StateWebSocketService(
            StateWebSocketClient client,
            ILogger<StateWebSocketService> logger) {
            _client = client;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            try {
                _logger.LogInformation("Starting StateWebSocketService...");
                await _client.ConnectAsync(); // 受信ループは client 内で fire-and-forget
                _logger.LogInformation("StateWebSocketService connected.");
            } catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) {
                _logger.LogWarning("StateWebSocketService start canceled.");
            } catch (Exception ex) {
                _logger.LogError(ex, "Failed to connect StateWebSocketClient.");
                // 起動失敗としてアプリ全体の起動を止めたいなら throw を残す
                // throw;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken) {
            try {
                _logger.LogInformation("Stopping StateWebSocketService...");
                await _client.DisconnectAsync();
                _logger.LogInformation("StateWebSocketService disconnected.");
            } catch (Exception ex) {
                // 既に Close/Abort 済みなどのケースは警告程度でOK
                _logger.LogWarning(ex, "Error while disconnecting StateWebSocketClient.");
            }
        }
    }
}

