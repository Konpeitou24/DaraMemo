using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Services.NotifyIcon
{
    /// <summary>
    /// 通知アイコン操作に関するインターフェース
    /// </summary>
    public interface INotifyIconService
    {
        /// <summary>
        /// 通知アイコン初期化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 通知アイコン表示
        /// </summary>
        void ShowNotifyIcon();

        /// <summary>
        /// 通知アイコン非表示
        /// </summary>
        void HideNotifyIcon();
        /// <summary>
        /// 終了時用の初期化を行う。
        /// </summary>
        void KillNotifyIcon();
    }
}
