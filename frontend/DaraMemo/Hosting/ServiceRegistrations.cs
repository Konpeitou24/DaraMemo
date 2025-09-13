using DaraMemo.Services.Api;
using DaraMemo.Services.Status;
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
            services.AddTransient<MainWindowViewModel>(); // MainWindowViewModel を DI コンテナに登録
            services.AddTransient<IStatusService, StatusService>();
            services.AddHttpClients();
        }
        private static void AddHttpClients(this IServiceCollection services) {
            services.AddHttpClient();
            services.AddHttpClient<StatusApiClient>();
        }

    }
}
