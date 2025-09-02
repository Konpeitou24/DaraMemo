using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Models.State {
    public class StateSegment {
        public string State { get; set; } = string.Empty; // ACTIVE, AFK, BREAK
        public DateTime StartedAt { get; set; }
    }

}
