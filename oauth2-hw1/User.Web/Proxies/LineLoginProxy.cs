using System.Net.Http.Headers;
using System.Text;
using User.Web.Controllers;
using User.Web.Models;

namespace User.Web.Proxies;

public class LineLoginProxy
{
    private readonly HttpClient _httpClient;
    private const string ClientId = "1660895431";
    private const string CallbackUrlFormat = "https://{0}/linelogin/callback";
    private const string ClientSecret = "580258b85a161bf8c14ebecc93422405";

    public LineLoginProxy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string GetLoginUrl(string requestHost)
    {
        return $"https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id={ClientId}&state=123123&scope=profile%20openid%20email&redirect_uri={string.Format(CallbackUrlFormat, requestHost)}";
    }

    public async Task<LineLoginResponse> ProcessCallback(string code, string requestHost)
    {
        var httpResponseMessage = await _httpClient.PostAsync("https://api.line.me/oauth2/v2.1/token", new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            { "code", code },
            { "grant_type", "authorization_code" },
            { "redirect_uri", string.Format(CallbackUrlFormat, requestHost) },
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "id_token_key_type", "JWK" },
        }));

        httpResponseMessage.EnsureSuccessStatusCode();
        var tokenResponse = await httpResponseMessage.Content.ReadFromJsonAsync<TokenResponse>();

        var verifyResponseMessage = await _httpClient.PostAsync("https://api.line.me/oauth2/v2.1/verify", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "id_token", tokenResponse.IdToken },
            { "client_id", ClientId }
        }));

        var tokenPayload = await verifyResponseMessage.Content.ReadFromJsonAsync<TokenPayload>();

        return new LineLoginResponse
        {
            Name = tokenPayload.Name,
            Email = tokenPayload.Email,
            LineId = tokenPayload.Subject,
            IdToken = tokenResponse.IdToken,
            AccessToken = tokenResponse.AccessToken
        };
    }

    public async Task RevokeToken(string lineLoginAccessToken)
    {
        var base64String = Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes($"{ClientId}:{ClientSecret}"));
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base64String}");
        
        var verifyResponseMessage = await _httpClient.PostAsync("https://api.line.me/oauth2/v2.1/revoke", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "access_token", lineLoginAccessToken },
        }));

        verifyResponseMessage.EnsureSuccessStatusCode();
    }
}