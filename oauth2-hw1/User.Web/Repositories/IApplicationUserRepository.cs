using User.Web.Models;

namespace User.Web.Repositories;

public interface IApplicationUserRepository
{
    void AddOrUpdate(ApplicationUser user);
    ApplicationUser GetByLineId(string lineId);
    List<ApplicationUser> GetAllSubscribedUser();
    void Register(ApplicationUser user);
}