using DaraMemo.Models.Dtos;
using DaraMemo.Services.Api;

namespace DaraMemo.Services.Status {
    public class StatusService: IStatusService {
        private readonly StatusApiClient _statusApiClient;
        public event EventHandler IsBusyChanged = delegate { };
        /// <summary>
        /// Gets or sets the currently active task.
        /// </summary>
        public Task? CurrentTask { get; private set; } =  null;
        public bool IsBusy => CurrentTask != null && !CurrentTask.IsCompleted;

        public StatusService(StatusApiClient statusApiClient) {
            _statusApiClient = statusApiClient;
        }
        public void FetchCurrentStatus(Action<StatusCurrentResponse>? onCompleted = null)
            => RunTask(_statusApiClient.GetCurrentStatusAsync, onCompleted);

        public void FetchStatusRecord(Action<StatusRecordResponse>? onCompleted = null)
            => RunTask(_statusApiClient.GetStatusRecordAsync, onCompleted);

        public void SetBreakStatus(Action<SimpleOkResponse>? onCompleted = null)
            => RunTask(_statusApiClient.SetBreakStatusAsync, onCompleted);

        public void ResetStatus(Action<SimpleOkResponse>? onCompleted = null)
            => RunTask(_statusApiClient.ResetStatusAsync, onCompleted);

        public void KillServer(Action<SimpleOkResponse>? onCompleted = null)
            => RunTask(_statusApiClient.KillServerAsync, onCompleted);


        private void RunTask<T>(Func<Task<T>> taskFactory, Action<T>? onCompleted) {
            CurrentTask = taskFactory()
                .ContinueWith(task => {
                    try {
                        if (task.Exception == null) {
                            T result = task.Result;
                            onCompleted?.Invoke(result);
                        } else {
                            Console.WriteLine("エラーが発生しました: " + task.Exception?.Message);
                        }
                    } finally {
                        CurrentTask = null;
                        IsBusyChanged.Invoke(this, EventArgs.Empty);
                    }
                });
        }

    }
}
