using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Models.Dtos {
    /// <summary>
    /// Represents a simple response indicating a successful operation.
    /// </summary>
    /// <remarks>This class is typically used to convey a basic success message or status in response to an
    /// operation. The <see cref="Result"/> field can be used to provide additional information about the
    /// success.</remarks>
    public class SimpleOkResponse: StatusResponse {
        public string Result = string.Empty;
    }
}
