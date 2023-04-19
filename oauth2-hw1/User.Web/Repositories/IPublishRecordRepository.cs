using User.Web.Models.DomainModels;

namespace User.Web.Repositories;

public interface IPublishRecordRepository
{
    Task Save(PublishRecord publishRecord);
    List<PublishRecord> GetAll();
}