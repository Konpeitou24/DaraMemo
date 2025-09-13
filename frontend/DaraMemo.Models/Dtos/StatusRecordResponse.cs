using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Models.Dtos {
    /// <summary>
    /// Represents a response containing status record details, including state and time metrics.
    /// </summary>
    /// <remarks>This class provides information about the current state and associated time metrics, such as
    /// active time, away-from-keyboard (AFK) time, and break time. All properties are initialized to empty strings by
    /// default.</remarks>
    public class StatusRecordResponse: StatusResponse {
        public string State = string.Empty;
        public string ActiveTime = string.Empty;
        public string AfkTime = string.Empty;
        public string BreakTime = string.Empty;
    }

}
