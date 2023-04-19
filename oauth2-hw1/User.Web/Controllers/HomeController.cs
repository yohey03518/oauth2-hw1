using Microsoft.AspNetCore.Mvc;

namespace User.Web.Controllers;

public class HomeController : Controller
{
    private readonly OnlineUserManager _onlineUserManager;

    public HomeController(OnlineUserManager onlineUserManager)
    {
        _onlineUserManager = onlineUserManager;
    }

    public IActionResult Index()
    {
        if (_onlineUserManager.IsUserLogin())
        {
            return RedirectToAction("Setting", "LineNotify");
        }

        return View();
    }
}