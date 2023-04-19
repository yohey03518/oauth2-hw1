using User.Web.Models;
using User.Web.Models.DomainModels;

namespace User.Web.Repositories;

public interface IPublishRecordRepository
{
    void Save(PublishRecord publishRecord);
    List<PublishRecord> GetAll();
}