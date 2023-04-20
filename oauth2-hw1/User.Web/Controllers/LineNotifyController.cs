using Microsoft.AspNetCore.Mvc;
using User.Web.Models;
using User.Web.Models.ViewModels;
using User.Web.Proxies;
using User.Web.Repositories;

namespace User.Web.Controllers;

public class LineNotifyController : Controller
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly LineNotifyProxy _lineNotifyProxy;
    private readonly OnlineUserManager _onlineUserManager;

    public LineNotifyController(OnlineUserManager onlineUserManager, IApplicationUserRepository applicationUserRepository, LineNotifyProxy lineNotifyProxy)
    {
        _onlineUserManager = onlineUserManager;
        _applicationUserRepository = applicationUserRepository;
        _lineNotifyProxy = lineNotifyProxy;
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
                RequestUserTokenUrl = _lineNotifyProxy.RequestUserTokenUrl(Request.Host.Value),
            };

            return View("Setting", lineNotifySettingModel);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Callback(string code)
    {
        var userLineNotifyAccessToken = await _lineNotifyProxy.GetAccessToken(code, Request.Host.Value);

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
        if (!user.IsSubscribed)
        {
            await _lineNotifyProxy.Revoke(user.LineNotifyAccessToken);
            user.LineNotifyAccessToken = string.Empty;
        }
        _applicationUserRepository.AddOrUpdate(user);
        return await Setting();
    }
}