using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaraMemo.Services;
using DaraMemo.Shell;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DaraMemo.Hosting {
    public static class BuilderRegistrations {
        public static IHost SuperBuild(this IHostBuilder builder) {
            return builder.ConfigureServices(ServiceRegistrations.ConfigureServices)
                .Build();
        }
        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services) {
            services.AddSingleton<MainWindow>(); // MainWindow を DI コンテナに登録
            services.AddSingleton<MainWindowViewModel>(); // MainWindowViewModel を DI コンテナに登録
            services.AddSingleton<INotifyIconService, NotifyIconService>();
        }
    }
}
