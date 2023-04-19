namespace User.Web.Extensions;

public static class UrlExtensions
{
    public static string GetRequestHostWithScheme(this HttpRequest httpRequest)
    {
        return httpRequest.Scheme + "://" + httpRequest.Host.Value;
    }
}