namespace User.Web.Models;

public class ApplicationUser
{
    public string LineId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsSubscribed { get; set; }
    public string LineNotifyAccessToken { get; set; }
    public string LineLoginIdToken { get; set; }
    public string LineLoginAccessToken { get; set; }

    public bool HasLineNotifyToken()
    {
        return !string.IsNullOrWhiteSpace(LineNotifyAccessToken);
    }

    public void ToggleSubscribe()
    {
        IsSubscribed = !IsSubscribed;
    }
}