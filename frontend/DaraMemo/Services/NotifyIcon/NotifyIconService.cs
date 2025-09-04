using DaraMemo.Shell;
using H.NotifyIcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DaraMemo.Services.NotifyIcon {
    public class NotifyIconService : INotifyIconService {
        private TaskbarIcon? _notifyIcon;

        public NotifyIconService() {
            MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;
            _notifyIcon = mainWindow?.TrayIcon;
        }
        void INotifyIconService.HideNotifyIcon() {
            _notifyIcon?.SetCurrentValue(TaskbarIcon.VisibilityProperty, Visibility.Collapsed);
        }

        void INotifyIconService.ShowNotifyIcon() {
            _notifyIcon?.SetCurrentValue(TaskbarIcon.VisibilityProperty, Visibility.Visible);
        }

    }
}
