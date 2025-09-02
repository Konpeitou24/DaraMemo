using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Models.Timeline {

    public class TimelineResponse {
        public string SessionId { get; set; } = string.Empty;
        public DateTime SessionStartedAt { get; set; }
        public List<TimeRange> Breaks { get; set; } = [];
        public List<TimeRange> Afks { get; set; } = [];
    }

}
