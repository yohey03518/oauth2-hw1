using System.Text;
using Microsoft.AspNetCore.Mvc;
using User.Web.Models;
using User.Web.Models.DomainModels;
using User.Web.Models.ViewModels;
using User.Web.Proxies;
using User.Web.Repositories;

namespace User.Web.Controllers;

public class PublishMessageController : Controller
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly LineNotifyProxy _lineNotifyProxy;
    private readonly IPublishRecordRepository _publishRecordRepository;

    public PublishMessageController(IApplicationUserRepository applicationUserRepository, LineNotifyProxy lineNotifyProxy, IPublishRecordRepository publishRecordRepository)
    {
        _applicationUserRepository = applicationUserRepository;
        _lineNotifyProxy = lineNotifyProxy;
        _publishRecordRepository = publishRecordRepository;
    }

    public IActionResult Index()
    {
        var list = _applicationUserRepository.GetAllSubscribedUser().Select(x => x.Name).ToList();

        var publishMessageIndexModel = new PublishMessageIndexModel()
        {
            SubscribedUsers = list,
            PublishRecords = _publishRecordRepository.GetAll()
        };

        return View("Index", publishMessageIndexModel);
    }

    [HttpPost]
    public async Task<IActionResult> Send(string message)
    {
        var responseMessageBuilder = new StringBuilder();
        var allSubscribedUser = _applicationUserRepository.GetAllSubscribedUser();

        var successUser = new List<string>();
        var failUserReason = new List<(string, string)>();

        foreach (var user in allSubscribedUser)
        {
            try
            {
                await _lineNotifyProxy.SendMessage(user.LineNotifyAccessToken, message);
                successUser.Add(user.Name);
                responseMessageBuilder.AppendLine($"Send to user {user.Name} successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                failUserReason.Add((user.Name, e.Message));
                responseMessageBuilder.AppendLine($"Send to user {user.Name} fail, message: {e.Message}");
            }
        }

        await _publishRecordRepository.Save(new PublishRecord(message, successUser, failUserReason));
        ViewBag.SendResultMessage = responseMessageBuilder.ToString();
        return Index();
    }
}