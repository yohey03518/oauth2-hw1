using Microsoft.AspNetCore.Mvc;
using User.Web.Models;
using User.Web.Repositories;

namespace User.Web.Controllers;

public class LineLoginController : Controller
{
    private const string ClientId = "1660895431";
    private const string CallbackUrl = "https://localhost:7122/linelogin/callback";
    private const string ClientSecret = "580258b85a161bf8c14ebecc93422405";
    private readonly HttpClient _httpClient;
    private readonly OnlineUserManager _onlineUserManager;
    private readonly IApplicationUserRepository _applicationUserRepository;

    public LineLoginController(HttpClient httpClient, OnlineUserManager onlineUserManager, IApplicationUserRepository applicationUserRepository)
    {
        _httpClient = httpClient;
        _onlineUserManager = onlineUserManager;
        _applicationUserRepository = applicationUserRepository;
    }

    [HttpGet]
    [HttpPost]
    public IActionResult Index()
    {
        if (_onlineUserManager.IsUserLogin())
        {
            return RedirectToAction("Setting", "LineNotify");
        }
        
        return Redirect($"https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id={ClientId}&state=123123&scope=profile%20openid%20email&redirect_uri={CallbackUrl}");
    }

    [HttpGet]
    public async Task<IActionResult> Callback(string code)
    {
        var httpResponseMessage = await _httpClient.PostAsync("https://api.line.me/oauth2/v2.1/token", new FormUrlEncodedContent(new Dictionary<string, string>()
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

        var verifyResponseMessage = await _httpClient.PostAsync("https://api.line.me/oauth2/v2.1/verify", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "id_token", tokenResponse.IdToken },
            { "client_id", ClientId }
        }));

        var payload = await verifyResponseMessage.Content.ReadFromJsonAsync<TokenPayload>();
        
        var applicationUser = new ApplicationUser
        {
            Name = payload.Name,
            Email = payload.Email,
            LineId = payload.Subject
        };

        // register if not exist
        _applicationUserRepository.AddOrUpdate(applicationUser);

        _onlineUserManager.DoLogin(applicationUser.LineId);
        return RedirectToAction("Setting", "LineNotify");
    }
}