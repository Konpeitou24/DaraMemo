using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Services.NotifyIcon {
    public interface INotifyIconService {
        /// <summary>
        /// 通知アイコン表示
        /// </summary>
        void ShowNotifyIcon();

        /// <summary>
        /// 通知アイコン非表示
        /// </summary>
        void HideNotifyIcon();
    }
}
