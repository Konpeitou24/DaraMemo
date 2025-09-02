using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Models.State {


    public class CurrentStateResponse {
        public string EffectiveState { get; set; } = string.Empty;
        public StateSegment Segment { get; set; } = new StateSegment();
        public DateTime ServerTime { get; set; }
    }


}
