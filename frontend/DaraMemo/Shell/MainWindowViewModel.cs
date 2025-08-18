using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;

namespace DaraMemo.Shell {
    public partial class MainWindowViewModel : ObservableObject {
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

        public MainWindowViewModel() {
        }

        // Reloadボタン

        [ObservableProperty]
        private string reloadButtonTootTip = MainResources.ReloadButtonToolTip;

        // --- RelayCommand ---
        [RelayCommand]
        private void Reload() {
            MessageBox.Show(MainResources.ReloadMessage);
        }
    }
}
