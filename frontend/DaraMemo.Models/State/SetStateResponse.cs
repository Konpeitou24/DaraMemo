using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Models.State {

    public class SetStateResponse {
        public StateSegment PrevSegment { get; set; } = new StateSegment();
        public StateSegment NewSegment { get; set; } = new StateSegment();
        public string EffectiveState { get; set; } = string.Empty;
        public DateTime ServerTime { get; set; }
        public bool Idempotent { get; set; }
    }

}
