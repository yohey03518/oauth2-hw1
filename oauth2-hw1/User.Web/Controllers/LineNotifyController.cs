using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using User.Web.Models;
using User.Web.Repositories;

namespace User.Web.Controllers;

public class LineNotifyController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly OnlineUserManager _onlineUserManager;
    private IApplicationUserRepository _applicationUserRepository;
    private LineNotifyProxy _lineNotifyProxy;

    public LineNotifyController(HttpClient httpClient, OnlineUserManager onlineUserManager, IApplicationUserRepository applicationUserRepository, LineNotifyProxy lineNotifyProxy)
    {
        _httpClient = httpClient;
        _onlineUserManager = onlineUserManager;
        _applicationUserRepository = applicationUserRepository;
        _lineNotifyProxy = lineNotifyProxy;
    }

    [HttpGet]
    [HttpPost]
    public IActionResult Login()
    {
        return Redirect(_lineNotifyProxy.GetLoginUrl());
    }

    public async Task<IActionResult> Setting()
    {
        if (_onlineUserManager.TryGetUser(out var user))
        {
            var lineNotifySettingModel = new LineNotifySettingModel
            {
                IsSubscribe = user.IsSubscribed,
                NeedNewAccessToken = !user.HasLineNotifyToken() || !await _lineNotifyProxy.IsValidLineNotifyToken(user.LineNotifyAccessToken),
                Name = user.Name,
                RequestUserTokenUrl = _lineNotifyProxy.RequestUserTokenUrl(),
            };

            return View("Setting", lineNotifySettingModel);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Callback(string code)
    {
        var userLineNotifyAccessToken = await _lineNotifyProxy.GetAccessToken(code);

        _onlineUserManager.TryGetUser(out var user);
        user.LineNotifyAccessToken = userLineNotifyAccessToken;
        _applicationUserRepository.AddOrUpdate(user);

        return await Setting();
    }

    [HttpPost]
    public async Task<IActionResult> ToggleSubscribe()
    {
        _onlineUserManager.TryGetUser(out var user);
        user.ToggleSubscribe();
        _applicationUserRepository.AddOrUpdate(user);
        return await Setting();
    }
}

public class LineNotifySettingModel
{
    public bool IsSubscribe { get; set; }
    public string RequestUserTokenUrl { get; set; }
    public string Name { get; set; }
    public bool NeedNewAccessToken { get; set; }
}