using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Enums {
    /// <summary>
    /// InSession 中の下位状態：作業中 or 離席中
    /// </summary>
    public enum ActivityState {
        Active,
        Afk
    }
}
