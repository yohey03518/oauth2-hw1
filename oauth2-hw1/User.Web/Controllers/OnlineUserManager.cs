using Microsoft.Extensions.Caching.Memory;
using User.Web.Models;
using User.Web.Models.DomainModels;
using User.Web.Repositories;

namespace User.Web.Controllers;

public class OnlineUserManager
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IMemoryCache memoryCache;

    public OnlineUserManager(IMemoryCache memoryCache, IApplicationUserRepository applicationUserRepository)
    {
        this.memoryCache = memoryCache;
        _applicationUserRepository = applicationUserRepository;
    }

    public HttpRequest HttpRequest { private get; set; } = null!;
    public HttpResponse HttpResponse { private get; set; } = null!;

    public bool IsUserLogin()
    {
        return TryGetUser(out _);
    }

    public void DoLogin(string lineId)
    {
        var newGuid = Guid.NewGuid();

        memoryCache.GetOrCreate(newGuid.ToString(), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            return lineId;
        });

        HttpResponse.Cookies.Append("token", newGuid.ToString());
    }

    public bool TryGetUser(out ApplicationUser user)
    {
        var token = GetLoginToken();
        user = null;
        var lineId = string.Empty;
        var tryGetValue = !string.IsNullOrWhiteSpace(token) && memoryCache.TryGetValue(token, out lineId!);

        if (tryGetValue)
        {
            user = _applicationUserRepository.GetByLineId(lineId);
        }

        return user != null && tryGetValue;
    }

    public void LogOut()
    {
        if (TryGetUser(out _))
        {
            memoryCache.Remove(GetLoginToken()!);
        }
    }

    private string? GetLoginToken()
    {
        var token = HttpRequest.Cookies["token"];
        return token;
    }
}