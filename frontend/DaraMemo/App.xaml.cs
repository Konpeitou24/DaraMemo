using DaraMemo.Hosting;
using DaraMemo.Services.Status;
using DaraMemo.Shell;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace DaraMemo {
    public partial class App : Application {
        private IHost? _host;
        private IStatusService? _statusService;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            _host = Host.CreateDefaultBuilder()
                .SuperBuild();

            _host.Start();

            // DI から MainWindow を解決して表示
            var window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();

            _statusService = _host.Services.GetRequiredService<StatusService>();
        }
        protected override async void OnExit(ExitEventArgs e) {
            // kill background process
            if (_statusService is not null) {
                _ = await _statusService.KillServerAsync();
            }

            if (_host is not null) {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
                _host.Dispose();
            }
            base.OnExit(e);
        }

    }
}
