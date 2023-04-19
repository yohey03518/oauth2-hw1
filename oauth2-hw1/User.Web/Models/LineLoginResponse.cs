namespace User.Web.Models;

public class LineLoginResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string LineId { get; set; }
    public string IdToken { get; set; }
    public string AccessToken { get; set; }
}