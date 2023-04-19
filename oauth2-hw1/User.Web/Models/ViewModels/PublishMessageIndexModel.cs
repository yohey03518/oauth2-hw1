using User.Web.Models.DomainModels;

namespace User.Web.Models.ViewModels;

public class PublishMessageIndexModel
{
    public List<string> SubscribedUsers { get; set; }
    public List<PublishRecord> PublishRecords { get; set; }
}