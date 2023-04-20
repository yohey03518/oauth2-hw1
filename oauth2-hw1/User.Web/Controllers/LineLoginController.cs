using Microsoft.AspNetCore.Mvc;
using User.Web.Models;
using User.Web.Models.DomainModels;
using User.Web.Proxies;
using User.Web.Repositories;

namespace User.Web.Controllers;

public class LineLoginController : Controller
{
    private readonly OnlineUserManager _onlineUserManager;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly LineLoginProxy _lineLoginProxy;

    public LineLoginController(OnlineUserManager onlineUserManager, IApplicationUserRepository applicationUserRepository, LineLoginProxy lineLoginProxy)
    {
        _onlineUserManager = onlineUserManager;
        _applicationUserRepository = applicationUserRepository;
        _lineLoginProxy = lineLoginProxy;
    }

    [HttpGet]
    [HttpPost]
    public IActionResult Index()
    {
        if (_onlineUserManager.IsUserLogin())
        {
            return RedirectToAction("Setting", "LineNotify");
        }

        return Redirect(_lineLoginProxy.GetLoginUrl(Request.Host.Value));
    }

    [HttpGet]
    public async Task<IActionResult> Callback(string code)
    {
        var lineLoginResponse = await _lineLoginProxy.ProcessCallback(code, Request.Host.Value);

        var applicationUser = new ApplicationUser
        {
            Name = lineLoginResponse.Name,
            Email = lineLoginResponse.Email,
            LineId = lineLoginResponse.LineId,
            LineLoginIdToken = lineLoginResponse.IdToken,
            LineLoginAccessToken = lineLoginResponse.AccessToken
        };

        _applicationUserRepository.Register(applicationUser);

        _onlineUserManager.DoLogin(applicationUser.LineId);
        return RedirectToAction("Setting", "LineNotify");
    }
}