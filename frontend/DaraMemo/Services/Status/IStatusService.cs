using DaraMemo.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Services.Status {
    public interface IStatusService {
        bool IsBusy { get; }
        event EventHandler IsBusyChanged;
        void FetchCurrentStatus(Action<StatusCurrentResponse>? onCompleted = null);
        void FetchStatusRecord(Action<StatusRecordResponse>? onCompleted = null);

        void SetBreakStatus(Action<SimpleOkResponse>? onCompleted = null);

        void ResetStatus(Action<SimpleOkResponse>? onCompleted = null);

        void KillServer(Action<SimpleOkResponse>? onCompleted = null);
    }
}
