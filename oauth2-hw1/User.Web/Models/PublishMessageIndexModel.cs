namespace User.Web.Models;

public class PublishMessageIndexModel
{
    public List<string> SubscribedUsers { get; set; }
    public List<PublishRecord> PublishRecords { get; set; }
}