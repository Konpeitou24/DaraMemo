using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DaraMemo.Shell;
using H.NotifyIcon;

namespace DaraMemo.Services.NotifyIcon
{
    public sealed class NotifyIconService : INotifyIconService
    {
        private TaskbarIcon? _notifyIcon;

        public void Initialize()
        {
            // MainWindowからTaskbarIconを取得
            if (Application.Current.MainWindow != null)
            {

                var taskbarIcon = GetNotifyIcon();
                if (taskbarIcon != null)
                {
                    _notifyIcon = taskbarIcon;
                }
            }
        }

        public void HideNotifyIcon()
        {
            _notifyIcon?.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Collapsed);
        }

        public void ShowNotifyIcon()
        {
            // _notifyIcon が null だったらNotifyIconを取得する
            _notifyIcon ??= GetNotifyIcon();

            // _notifyIcon が null でなけれは表示する
            _notifyIcon?.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Visible);
        }

        /// <summary>
        /// NotifyIconを取得
        /// </summary>
        /// <returns></returns>
        private TaskbarIcon? GetNotifyIcon()
        {
            // MainWindowからNotifyIconを取得
            var mainWindow = Application.Current.MainWindow as MainWindow;
            return mainWindow?.TrayIcon;
        }
    }
}
