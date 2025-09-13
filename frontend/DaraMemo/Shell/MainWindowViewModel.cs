using System;
using System.ComponentModel;
using System.Runtime.InteropServices.Swift;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DaraMemo.Enums;
using DaraMemo.Services.NotifyIcon;
using DaraMemo.Services.Status;

namespace DaraMemo.Shell {
    public partial class MainWindowViewModel : ObservableObject {

        private const double TimerInterval = 1;

        private readonly INotifyIconService _notifyIconService;

        private readonly IStatusService _statusService;

        private readonly DispatcherTimer _dispatcherTimer;
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
        private ImageSource _taskbarIconImageSource = (ImageSource)Application.Current.Resources[Status.Afk.GetIcon()];

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
        private string? activeTimeSum ;

        [ObservableProperty]
        private string afkTimeSumTitle = MainResources.AfkTimeSumTitle;

        [ObservableProperty]
        private string? afkTimeSum;

        [ObservableProperty]
        private string breakTimeSumTitle = MainResources.BreakTimeSumTitle;

        [ObservableProperty]
        private string? breakTimeSum;


        private bool IsBusy { get => _statusService.IsBusy!; }

        // Reloadボタン

        [ObservableProperty]
        private string reloadButtonToolTip = MainResources.ReloadButtonToolTip;

        public MainWindowViewModel(INotifyIconService notifyIconService, IStatusService statusService, DispatcherTimer dispatcherTimer) {
            _notifyIconService = notifyIconService;
            _statusService = statusService;
            _dispatcherTimer = dispatcherTimer;

            Initialize();
        }
        #region Initialize
        private void Initialize() {
            InitializeDispatcherTimer();
            InitializeStatusService();
        }
        private void InitializeDispatcherTimer() {
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(TimerInterval);
            _dispatcherTimer.Tick += OnTickDispatcherTimer;
        }
        private void InitializeStatusService() {
            _statusService.IsBusyChanged += OnStatusServiceTaskIsBusyChanged;
        }
        #endregion
        private void OnTickDispatcherTimer(object? sender, EventArgs e) {
            Reload();
        }
        private void  OnStatusServiceTaskIsBusyChanged(object? sender, EventArgs e) {
            ToggleBreakCommand.NotifyCanExecuteChanged();
            ReloadCommand.NotifyCanExecuteChanged();
        }
        // --- RelayCommand ---
        [RelayCommand(CanExecute = nameof(IsBusy))]
        private void Reload() {
            SetCurrentStatus();
            SetCurrentRecord();
        }

        [RelayCommand(CanExecute = nameof(IsBusy))]
        private void ToggleBreak() {
            _statusService.SetBreakStatus();
        }

        [RelayCommand]
        private void WindowClosing(CancelEventArgs e) {
            // IsTaskTrayがtrueの場合、タスクトレイに格納して閉じるのをキャンセル
            if (IsClosedTaskTray) {
                e.Cancel = true; // 閉じるのをキャンセル

                var window = Application.Current.MainWindow;
                if (window != null) {
                    window.Hide(); // ウィンドウを非表示
                    // タスクトレイアイコンを表示
                    _notifyIconService.ShowNotifyIcon();
                }
            }
            // IsTaskTrayがfalseの場合は通常通り閉じる（e.Cancelはfalseのまま）
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
            _notifyIconService.KillNotifyIcon();
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
        private void SetCurrentStatus() {
            if (_statusService.IsBusy) return;
            _statusService.FetchCurrentStatus(
                onCompleted => {
                    if (onCompleted is null) return;
                    if (LookupStatus(onCompleted.State) == Status.Active) {
                        SetStatusActive();
                    } else if (LookupStatus(onCompleted.State) == Status.Afk) {
                        SetStatusAfk();
                    } else if (LookupStatus(onCompleted.State) == Status.Break) {
                        SetStatusBreak();
                    }
                }
            );
        }
        private void SetCurrentRecord() {
            _statusService.FetchStatusRecord(
                onCompleted => {
                    if (onCompleted is null) return;
                    if (onCompleted.ActiveTime != null) ActiveTimeSum = onCompleted.ActiveTime;
                    if (onCompleted.AfkTime != null) AfkTimeSum = onCompleted.AfkTime;
                    if (onCompleted.BreakTime != null) BreakTimeSum = onCompleted.BreakTime;
                }
            );
        }
        private void SetStatusActive() {
            TaskbarIconImageSource = Status.Active.GetIcon();
            AfkStatus = Status.Active.ToString();
        }
        private void SetStatusAfk() {
            TaskbarIconImageSource = Status.Afk.GetIcon();
            AfkStatus = Status.Afk.ToString();
        }
        private void SetStatusBreak() {
            TaskbarIconImageSource = Status.Break.GetIcon();
            AfkStatus = Status.Break.ToString();
        }
        private static Status? LookupStatus(string status) {
            foreach (Status s in Enum.GetValues<Status>()) {
                if (status.Equals(s.ToString(), StringComparison.CurrentCultureIgnoreCase)) {
                    return s;
                }
            }
            return null;
        }

    }
}
