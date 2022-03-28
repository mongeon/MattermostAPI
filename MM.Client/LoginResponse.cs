using System.Text.Json.Serialization;

namespace MM.Api;
public class LoginResponse
{
    [JsonPropertyName("id")]
    public string UserId { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }
}
