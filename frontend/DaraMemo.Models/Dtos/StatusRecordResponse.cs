using System.Text.Json.Serialization;

namespace DaraMemo.Models.Dtos {


    public class StatusRecordResponse : StatusResponse {
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("active_time")]
        public string ActiveTime { get; set; } = string.Empty;

        [JsonPropertyName("afk_time")]
        public string AfkTime { get; set; } = string.Empty;

        [JsonPropertyName("break_time")]
        public string BreakTime { get; set; } = string.Empty;
    }

}
