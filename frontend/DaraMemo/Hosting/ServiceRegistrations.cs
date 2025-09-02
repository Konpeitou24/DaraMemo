using DaraMemo.Services.WebSocket;
using DaraMemo.Shell;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Hosting {
    public static class ServiceRegistrations {
        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services) {
            services.AddSingleton<MainWindow>(); // MainWindow を DI コンテナに登録
            services.AddSingleton<MainWindowViewModel>(); // MainWindowViewModel を DI コンテナに登録
            services.AddWebSocket(context);
        }
        private static void AddWebSocket(this IServiceCollection services, HostBuilderContext context) {
            services.Configure<StateWebSocketOptions>(
                        context.Configuration.GetSection("Api"));
            services.AddSingleton<StateWebSocketClient>();
            services.AddHostedService<StateWebSocketService>();
        }
    }
}
