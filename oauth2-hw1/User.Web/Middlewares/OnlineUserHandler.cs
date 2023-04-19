using User.Web.Controllers;

namespace User.Web.Middlewares;

public class OnlineUserHandler
{
    private readonly RequestDelegate _next;

    public OnlineUserHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, OnlineUserManager onlineUserManager)
    {
        onlineUserManager.HttpRequest = context.Request;
        onlineUserManager.HttpResponse = context.Response;
        await _next(context);
            
    }
}