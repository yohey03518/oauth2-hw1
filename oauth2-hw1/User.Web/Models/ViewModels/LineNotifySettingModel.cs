namespace User.Web.Models.ViewModels;

public class LineNotifySettingModel
{
    public bool IsSubscribe { get; set; }
    public string RequestUserTokenUrl { get; set; }
    public string Name { get; set; }
    public bool NeedNewAccessToken { get; set; }
}