using System.Text.Json.Serialization;

namespace MM.Api;

public interface ISettings
{
    [JsonIgnore]
    string ApiUrl { get; set; }

    [JsonPropertyName("login_id")]
    string Login { get; set; }
    [JsonPropertyName("password")]
    string Password { get; set; }
}
