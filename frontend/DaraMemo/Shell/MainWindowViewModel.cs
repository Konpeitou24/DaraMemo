using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DaraMemo.Services.NotifyIcon;
using System;
using System.ComponentModel;
using System.Windows;

namespace DaraMemo.Shell {
    public partial class MainWindowViewModel : ObservableObject {

        private readonly INotifyIconService _notifyIconService;

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

#region タスクトレイアイコンに関するフラグ
        /// <summary>
        /// 閉じるボタンでタスクトレイに格納する
        /// </summary>
        [ObservableProperty]
        private bool isTaskTray;
        /// <summary>
        /// 最小化時にタスクトレイに格納する
        /// </summary>
        [ObservableProperty]
        private bool isMinimization;

#endregion タスクトレイアイコンに関するフラグ

        public MainWindowViewModel(INotifyIconService notifyIconService) {
            _notifyIconService = notifyIconService;
            // アプリ起動時に通知アイコンを表示
            _notifyIconService.ShowNotifyIcon();
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
        private void WindowClosing(CancelEventArgs e) {
            if (IsTaskTray) {
                e.Cancel = true; // 閉じる動作をキャンセル
                // ウィンドウを非表示にする(これあんまりよくないんだけど今回は説明を割愛しますね。すみません。)
                Application.Current.MainWindow?.Hide();
                // タスクトレイアイコンを表示
                _notifyIconService.ShowNotifyIcon();
            }
        }
        [RelayCommand]
        private void ExitApplication() {
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
        private void ShowWindow() {
            // ウィンドウを表示し、前面に持ってくる
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Activate();
            Application.Current.MainWindow.Focus();

            // ウィンドウが表示されたらタスクトレイアイコンを非表示にする
            _notifyIconService.HideNotifyIcon();
        }

        [RelayCommand]
        private void WindowStateChanged() {
            var window = Application.Current.MainWindow;
            if (window == null) return;

            if (window.WindowState == WindowState.Minimized) {
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
