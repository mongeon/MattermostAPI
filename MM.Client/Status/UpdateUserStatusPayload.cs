using System.Text.Json.Serialization;

namespace MM.Api.Status;
public class UpdateUserStatusPayload
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
    [JsonPropertyName("status")]
    public UserStatus Status { get; set; }

    [JsonPropertyName("dnd_end_time")]
    public int DndEndTime { get; set; }
}
