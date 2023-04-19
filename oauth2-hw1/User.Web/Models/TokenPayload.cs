using System.Text.Json.Serialization;

namespace User.Web.Models;

public class TokenPayload
{
    [JsonPropertyName("iss")]
    public string Issuer { get; set; }

    [JsonPropertyName("sub")]
    public string Subject { get; set; }

    [JsonPropertyName("aud")]
    public string Audience { get; set; }

    [JsonPropertyName("exp")]
    public long ExpirationTime { get; set; }

    [JsonPropertyName("iat")]
    public long IssuedAt { get; set; }

    [JsonPropertyName("amr")]
    public List<string> AuthenticationMethods { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("picture")]
    public string Picture { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}