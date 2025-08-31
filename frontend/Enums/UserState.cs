using System.Text.Json.Serialization;

namespace DaraMemo.Models.Enums
{
    [JsonConverter(typeof(UserStateJsonConverter))]
    public enum UserState
    {
        Active,
        Afk,
        Break
    }
}
