using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DaraMemo.Models.Contracts
{
    /// <summary>共通エラーのラッパー</summary>
    public sealed class ErrorResponse
    {
        [JsonPropertyName("error")]
        public ErrorBody Error { get; set; } = new();
    }

    public sealed class ErrorBody
    {
        /// <summary>invalid_argument | not_found | conflict など</summary>
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("details")]
        public Dictionary<string, object>? Details { get; set; }
    }
}
