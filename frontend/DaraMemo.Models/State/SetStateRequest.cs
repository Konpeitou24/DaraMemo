using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Models.State {

    public class SetStateRequest {
        public string SessionId { get; set; } = string.Empty;
        public string NewState { get; set; } = string.Empty; // ACTIVE, AFK, BREAK
        public DateTime Timestamp { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string IdempotencyKey { get; set; } = string.Empty;
    }

}
