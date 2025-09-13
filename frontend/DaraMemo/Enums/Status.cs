using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Enums {

    public enum Status {
        [Description("アクティブ")]
        Active,
        [Description("離席中")]
        Afk,
        [Description("休憩中")]
        Break
    }
}
