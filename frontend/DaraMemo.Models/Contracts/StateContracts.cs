using System;
using System.Text.Json.Serialization;

namespace DaraMemo.Models.Contracts
{
    /// <summary>作業サマリ（秒）</summary>
    public sealed class SessionSummarySeconds
    {
        [JsonPropertyName("work_seconds")]
        public int WorkSeconds { get; set; }

        [JsonPropertyName("afk_seconds")]
        public int AfkSeconds { get; set; }

        [JsonPropertyName("break_seconds")]
        public int BreakSeconds { get; set; }
    }

    /// <summary>セーブ指示 リクエスト</summary>
    public sealed class SaveSessionRequest
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("summary")]
        public SessionSummarySeconds Summary { get; set; } = new();
    }

    /// <summary>セーブ指示 レスポンス</summary>
    public sealed class SaveSessionResponse
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("stored")]
        public bool Stored { get; set; }

        /// <summary>保存時刻（UTC）</summary>
        [JsonPropertyName("saved_at")]
        public DateTimeOffset SavedAt { get; set; }

        /// <summary>ログ保存先など返せる場合</summary>
        [JsonPropertyName("path")]
        public string? Path { get; set; }
    }

    /// <summary>終了指示 リクエスト</summary>
    public sealed class EndSessionRequest
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("ended_at")]
        public DateTimeOffset EndedAt { get; set; }

        [JsonPropertyName("summary")]
        public SessionSummarySeconds Summary { get; set; } = new();
    }

    /// <summary>終了指示 レスポンス</summary>
    public sealed class EndSessionResponse
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("stored")]
        public bool Stored { get; set; }

        /// <summary>ログ保存先など返せる場合</summary>
        [JsonPropertyName("path")]
        public string? Path { get; set; }
    }
}
