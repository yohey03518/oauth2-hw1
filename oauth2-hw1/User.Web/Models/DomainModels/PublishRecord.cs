namespace User.Web.Models.DomainModels;

public class PublishRecord
{
    public PublishRecord() { }
    public PublishRecord(string message, List<string> successUser, List<(string, string)> failUserReason)
    {
        Message = message;
        SuccessUser = successUser;
        FailUserReason = failUserReason;
        PublishTime = DateTime.Now;
    }

    public string Message { get; set; }
    public List<string> SuccessUser { get; set; }
    public List<(string, string)> FailUserReason { get; set; }

    public DateTime PublishTime { get; set; }
}