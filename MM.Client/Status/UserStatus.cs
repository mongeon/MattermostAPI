using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MM.Api.Status;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum UserStatus
{
    Unknown = 0,
    [EnumMember(Value = "online")]
    Online,
    [EnumMember(Value = "away")]
    Away,
    [EnumMember(Value = "dnd")]
    DoNotDisturb,
    [EnumMember(Value = "offline")]
    Offline
}
