using System;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DaraMemo.Services.NotifyIcon;

namespace DaraMemo.Shell {
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly INotifyIconService _notifyIconService;
        /// <summary>
        /// 閉じるボタンでタスクトレイに格納する
        /// </summary>
        [ObservableProperty]
        private bool _isClosedTaskTray;

        /// <summary>
        /// 最小化時にタスクトレイに格納する
        /// </summary>
        [ObservableProperty]
        private bool _isMinimization;

        
        [ObservableProperty]
        private string title = MainResources.Title;

        // --- Status セクション ---
        [ObservableProperty]
        private string statusHeaderTitle = MainResources.StatusHeaderTitle;

        [ObservableProperty]
        private string afkStatusTitle = MainResources.AfkStatusTitle;

        [ObservableProperty]
        private string afkStatus = "未計測";

        [ObservableProperty]
        private string breakButtonTitle = MainResources.BreakButtonTitle;

        // --- Record セクション ---
        [ObservableProperty]
        private string recordHeaderTitle = MainResources.RecordHeaderTitle;

        [ObservableProperty]
        private string activeTimeSumTitle = MainResources.ActiveTimeSumTitle;

        [ObservableProperty]
        private TimeSpan activeTimeSum = TimeSpan.Zero;

        [ObservableProperty]
        private string afkTimeSumTitle = MainResources.AfkTimeSumTitle;

        [ObservableProperty]
        private TimeSpan afkTimeSum = TimeSpan.Zero;

        [ObservableProperty]
        private string breakTimeSumTitle = MainResources.BreakTimeSumTitle;

        [ObservableProperty]
        private TimeSpan breakTimeSum = TimeSpan.Zero;

        public MainWindowViewModel(INotifyIconService notifyIconService) {
            _notifyIconService = notifyIconService;
        }

        // Reloadボタン

        [ObservableProperty]
        private string reloadButtonTootTip = MainResources.ReloadButtonToolTip;

        // --- RelayCommand ---
        [RelayCommand]
        private void Reload() {
            MessageBox.Show(MainResources.ReloadMessage);
        }

        [RelayCommand]
        private void WindowClosing(CancelEventArgs e)
        {
            // IsTaskTrayがtrueの場合、タスクトレイに格納して閉じるのをキャンセル
            if (IsClosedTaskTray)
            {
                e.Cancel = true; // 閉じるのをキャンセル

                var window = Application.Current.MainWindow;
                if (window != null)
                {
                    window.Hide(); // ウィンドウを非表示
                    // タスクトレイアイコンを表示
                    _notifyIconService.ShowNotifyIcon();
                }
            }
            // IsTaskTrayがfalseの場合は通常通り閉じる（e.Cancelはfalseのまま）
        }

        [RelayCommand]
        private void ExitApplication()
        {
            // Hide のままだと確認ダイアログが一瞬表示されて非表示になるので Activateメソッドを実行する
            Application.Current.MainWindow.Activate();

            var messege = "アプリケーションを終了しますか？";
            var caption = "確認";

            var result = MessageBox.Show(messege, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No) return;

            _notifyIconService.HideNotifyIcon();
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void ShowWindow()
        {
            // ウィンドウを表示し、前面に持ってくる
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Activate();
            Application.Current.MainWindow.Focus();

            // ウィンドウが表示されたらタスクトレイアイコンを非表示にする
            _notifyIconService.HideNotifyIcon();
        }

        [RelayCommand]
        private void WindowStateChanged()
        {
            var window = Application.Current.MainWindow;
            if (window == null) return;

            if (window.WindowState == WindowState.Minimized)
            {
                // 最小化時にタスクトレイに格納するか否か
                if (!IsMinimization) return;

                // ウィンドウを非表示
                Application.Current.MainWindow.Hide();
                // 通知アイコンを表示
                _notifyIconService.ShowNotifyIcon();
            }
        }
    }
}
