using DaraMemo.Hosting;
using DaraMemo.Shell;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace DaraMemo {
    public partial class App : Application {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .SuperBuild();

            _host.Start();

            // DI から MainWindow を解決して表示
            var window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
        }
        protected override async void OnExit(ExitEventArgs e) {
            if (_host is not null) {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
                _host.Dispose();
            }
            base.OnExit(e);
        }
    }
}
