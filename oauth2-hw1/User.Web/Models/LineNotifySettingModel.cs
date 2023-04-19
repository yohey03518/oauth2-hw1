namespace User.Web.Models;

public class LineNotifySettingModel
{
    public bool IsSubscribe { get; set; }
    public string RequestUserTokenUrl { get; set; }
    public string Name { get; set; }
    public bool NeedNewAccessToken { get; set; }
}