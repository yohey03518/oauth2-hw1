using User.Web.Models;

namespace User.Web.Repositories;

public class InMemoryUserRepository : IApplicationUserRepository
{
    private static readonly List<ApplicationUser> Users = new();
    public void AddOrUpdate(ApplicationUser user)
    {
        var existUser = Users.FirstOrDefault(x => x.LineId == user.LineId);
        if (existUser == null)
        {
            Users.Add(user);
        }
        else
        {
            existUser.Name = user.Name;
            existUser.LineNotifyAccessToken = user.LineNotifyAccessToken;
            existUser.IsSubscribed = user.IsSubscribed;
        }
    }

    public ApplicationUser GetByLineId(string lineId)
    {
        return Users.FirstOrDefault(x => x.LineId == lineId)!;
    }

    public List<ApplicationUser> GetAllSubscribedUser()
    {
        return Users.Where(x => x.IsSubscribed).ToList();
    }

    public void Register(ApplicationUser user)
    {
        var existUser = Users.FirstOrDefault(x => x.LineId == user.LineId);
        if (existUser == null)
        {
            Users.Add(user);
        }
        else
        {
            existUser.Name = user.Name;
            existUser.Email = user.Email;
            existUser.LineLoginIdToken = user.LineLoginIdToken;
            existUser.LineLoginAccessToken = user.LineLoginAccessToken;
        }
    }
}