using User.Web.Models;

namespace User.Web.Repositories;

public class InMemoryPublishRecordRepository : IPublishRecordRepository
{
    private static List<PublishRecord> Records = new();
    public void Save(PublishRecord publishRecord)
    {
        Records.Add(publishRecord);
    }

    public List<PublishRecord> GetAll()
    {
        return Records;
    }
}