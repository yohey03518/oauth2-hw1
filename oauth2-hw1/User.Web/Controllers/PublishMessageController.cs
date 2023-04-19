using System.Text;
using Microsoft.AspNetCore.Mvc;
using User.Web.Repositories;

namespace User.Web.Controllers;

public class PublishMessageController : Controller
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly LineNotifyProxy _lineNotifyProxy;

    public PublishMessageController(IApplicationUserRepository applicationUserRepository, LineNotifyProxy lineNotifyProxy)
    {
        _applicationUserRepository = applicationUserRepository;
        _lineNotifyProxy = lineNotifyProxy;
    }

    public IActionResult Index()
    {
        var list = _applicationUserRepository.GetAllSubscribedUser().Select(x => x.Name).ToList();
        return View("Index", list);
    }

    [HttpPost]
    public async Task<IActionResult> Send(string message)
    {
        var responseMessageBuilder = new StringBuilder();
        var allSubscribedUser = _applicationUserRepository.GetAllSubscribedUser();
        
        foreach (var user in allSubscribedUser)
        {
            try
            {
                await _lineNotifyProxy.SendMessage(user.LineNotifyAccessToken, message);
                responseMessageBuilder.AppendLine($"Send to user {user.Name} successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                responseMessageBuilder.AppendLine($"Send to user {user.Name} fail, message: {e.Message}");
            }
        }

        ViewBag.SendResultMessage = responseMessageBuilder.ToString();
        return Index();
    }
}