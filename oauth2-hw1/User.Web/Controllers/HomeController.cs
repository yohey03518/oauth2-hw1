using Microsoft.AspNetCore.Mvc;
using User.Web.Proxies;

namespace User.Web.Controllers;

public class HomeController : Controller
{
    private readonly OnlineUserManager _onlineUserManager;
    private readonly LineLoginProxy _lineLoginProxy;

    public HomeController(OnlineUserManager onlineUserManager, LineLoginProxy lineLoginProxy)
    {
        _onlineUserManager = onlineUserManager;
        _lineLoginProxy = lineLoginProxy;
    }

    public IActionResult Index()
    {
        if (_onlineUserManager.IsUserLogin())
        {
            return RedirectToAction("Setting", "LineNotify");
        }

        return View("Index");
    }

    public async Task<IActionResult> LogOut()
    {
        if (_onlineUserManager.TryGetUser(out var user))
        {
            _onlineUserManager.LogOut();
            await _lineLoginProxy.RevokeToken(user.LineLoginAccessToken);
        }

        return Index();
    }
}