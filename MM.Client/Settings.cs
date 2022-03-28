using System.Text.Json.Serialization;

namespace MM.Api;
public class Settings : ISettings
{
    [JsonPropertyName("login_id")]
    public string Login { get; set; } = string.Empty;
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonIgnore]
    public string ApiUrl { get; set; } = string.Empty;
}
