using User.Web.Models;

namespace User.Web.Repositories;

public interface IPublishRecordRepository
{
    void Save(PublishRecord publishRecord);
    List<PublishRecord> GetAll();
}