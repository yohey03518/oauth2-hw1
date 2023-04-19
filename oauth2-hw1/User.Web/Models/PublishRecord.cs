namespace User.Web.Models;

public class PublishRecord
{
    public PublishRecord(string message, List<string> successUser, List<(string, string)> failUserReason)
    {
        Message = message;
        SuccessUser = successUser;
        FailUserReason = failUserReason;
        PublishTime = DateTime.Now;
    }

    public string Message { get; }
    public List<string> SuccessUser { get; }
    public List<(string, string)> FailUserReason { get; }

    public DateTime PublishTime { get; }
}