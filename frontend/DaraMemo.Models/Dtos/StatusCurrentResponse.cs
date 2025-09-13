using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DaraMemo.Models.Dtos {
    /// <summary>
    /// Represents the response containing the current status of an operation or entity.
    /// </summary>
    /// <remarks>This class provides a simple structure to convey the current state as a string.</remarks>
    public class StatusCurrentResponse : StatusResponse {
        public string? Value { get; set; }
    }

}
