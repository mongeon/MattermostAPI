using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MM.Api.Status;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum UserStatus
{
    Unknown = 0,
    [EnumMember(Value = "online")]
    Online = 1,
    [EnumMember(Value = "away")]
    Away = 2,
    [EnumMember(Value = "dnd")]
    DoNotDisturb = 3,
    [EnumMember(Value = "offline")]
    Offline = 4
}
