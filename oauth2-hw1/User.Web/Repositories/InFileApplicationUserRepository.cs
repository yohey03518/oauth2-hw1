using System.Text.Json;
using User.Web.Models.DomainModels;

namespace User.Web.Repositories;

public class InFileApplicationUserRepository : IApplicationUserRepository
{
    private const string FilePath = "applicationUser.json";

    public InFileApplicationUserRepository()
    {
        CreateFileIfNotExist();
    }
    public void AddOrUpdate(ApplicationUser user)
    {
        var applicationUsers = GetAll();
        
        var existUser = applicationUsers.FirstOrDefault(x => x.LineId == user.LineId);
        if (existUser == null)
        {
            applicationUsers.Add(user);
        }
        else
        {
            existUser.Name = user.Name;
            existUser.LineNotifyAccessToken = user.LineNotifyAccessToken;
            existUser.IsSubscribed = user.IsSubscribed;
        }
        
        Save(applicationUsers);
    }

    private static void Save(List<ApplicationUser> applicationUsers)
    {
        using var streamWriter = new StreamWriter(FilePath);
        streamWriter.Write(JsonSerializer.Serialize(applicationUsers));
    }

    public ApplicationUser GetByLineId(string lineId)
    {
        var applicationUsers = GetAll();
        return applicationUsers.FirstOrDefault(x => x.LineId == lineId)!;
    }

    public List<ApplicationUser> GetAllSubscribedUser()
    {
        return GetAll().Where(x => x.IsSubscribed).ToList();
    }

    public void Register(ApplicationUser user)
    {
        var applicationUsers = GetAll();
        var existUser = applicationUsers.FirstOrDefault(x => x.LineId == user.LineId);
        if (existUser == null)
        {
            applicationUsers.Add(user);
        }
        else
        {
            existUser.Name = user.Name;
            existUser.Email = user.Email;
            existUser.LineLoginIdToken = user.LineLoginIdToken;
            existUser.LineLoginAccessToken = user.LineLoginAccessToken;
        }
        Save(applicationUsers);
    }

    private List<ApplicationUser> GetAll()
    {
        using var streamReader = new StreamReader(FilePath);
        var readToEnd = streamReader.ReadToEnd();
        return JsonSerializer.Deserialize<List<ApplicationUser>>(readToEnd)!;
    }
    
    private static void CreateFileIfNotExist()
    {
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath).Close();
            using var streamWriter = new StreamWriter(FilePath);
            streamWriter.Write("[]");
        }
    }
}