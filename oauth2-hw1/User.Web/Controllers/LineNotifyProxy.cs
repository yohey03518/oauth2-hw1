using System.Net.Http.Headers;
using User.Web.Models;

namespace User.Web.Controllers;

public class LineNotifyProxy
{
    private const string ClientId = "obKA6oUcwpV0nMgyhKmZKt";
    private const string CallbackUrl = "https://localhost:7122/linenotify/callback";
    private const string ClientSecret = "YkvNpsYHLUxs8Ql5JRVtcFWIfbxJ4YJPsSVMdQTisQf";
    private readonly HttpClient _httpClient;

    public LineNotifyProxy(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public string GetLoginUrl()
    {
        return $"https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id={ClientId}&state=123123&scope=profile%20openid%20email&redirect_uri={CallbackUrl}";
    }

    public string RequestUserTokenUrl()
    {
        return $"https://notify-bot.line.me/oauth/authorize?response_type=code&client_id={ClientId}&state=123123&scope=notify&redirect_uri={CallbackUrl}";
    }

    public async Task<string> GetAccessToken(string code)
    {
        var httpResponseMessage = await _httpClient.PostAsync("https://notify-bot.line.me/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            { "code", code },
            { "grant_type", "authorization_code" },
            { "redirect_uri", CallbackUrl },
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "id_token_key_type", "JWK" },
        }));

        httpResponseMessage.EnsureSuccessStatusCode();
        var tokenResponse = await httpResponseMessage.Content.ReadFromJsonAsync<TokenResponse>();
        return tokenResponse.AccessToken;
    }

    public async Task<bool> IsValidLineNotifyToken(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        return (await _httpClient.GetAsync("https://notify-api.line.me/api/status")).IsSuccessStatusCode;
    }

    public async Task SendMessage(string accessToken, string message)
    {
        if (!await IsValidLineNotifyToken(accessToken))
        {
            throw new InvalidOperationException("invalid token");
        }
        
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        
        var verifyResponseMessage = await _httpClient.PostAsync("https://notify-api.line.me/api/notify", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "message", message }
        }));

        verifyResponseMessage.EnsureSuccessStatusCode();
    }
}