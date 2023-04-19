using User.Web.Models.DomainModels;

namespace User.Web.Repositories;

public class InMemoryPublishRecordRepository : IPublishRecordRepository
{
    private static List<PublishRecord> Records = new();
    public Task Save(PublishRecord publishRecord)
    {
        Records.Add(publishRecord);
        return Task.CompletedTask;
    }

    public List<PublishRecord> GetAll()
    {
        return Records;
    }
}