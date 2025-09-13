using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Models.Dtos {
    /// <summary>
    /// Represents the response containing the current status of an operation or entity.
    /// </summary>
    /// <remarks>This class provides a simple structure to convey the current state as a string.</remarks>
    public class StatusCurrentResponse {
        public string State = string.Empty;
    }
    /// <summary>
    /// Represents a simple response indicating a successful operation.
    /// </summary>
    /// <remarks>This class is typically used to convey a basic success message or status in response to an
    /// operation. The <see cref="Result"/> field can be used to provide additional information about the
    /// success.</remarks>
    public class SimpleOkResponse {
        public string Result = string.Empty;
    }
    /// <summary>
    /// Represents a response containing status record details, including state and time metrics.
    /// </summary>
    /// <remarks>This class provides information about the current state and associated time metrics, such as
    /// active time, away-from-keyboard (AFK) time, and break time. All properties are initialized to empty strings by
    /// default.</remarks>
    public class StatusRecordResponse {
        public string State = string.Empty;
        public string ActiveTime = string.Empty;
        public string AfkTime = string.Empty;
        public string BreakTime = string.Empty;
    }

}
