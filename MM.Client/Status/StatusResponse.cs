using System.Text.Json.Serialization;

namespace MM.Api.Status;
public class StatusResponse
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
    [JsonPropertyName("status")]
    public UserStatus Status { get; set; }
}
