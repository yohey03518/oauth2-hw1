using Microsoft.Extensions.Caching.Memory;
using User.Web.Models;
using User.Web.Repositories;

namespace User.Web.Controllers;

public class OnlineUserManager
{
    private readonly IMemoryCache memoryCache;
    private readonly IApplicationUserRepository _applicationUserRepository;

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
        var token = HttpRequest.Cookies["token"];
        user = null!;
        string lineId = string.Empty;
        var tryGetValue = !string.IsNullOrWhiteSpace(token) && memoryCache.TryGetValue(token, out lineId!);
        if (tryGetValue)
        {
            user = _applicationUserRepository.GetByLineId(lineId);
        }
        return tryGetValue;
    }
}