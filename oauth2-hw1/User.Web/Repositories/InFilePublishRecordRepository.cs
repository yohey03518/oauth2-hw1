using System.Text.Json;
using User.Web.Models.DomainModels;

namespace User.Web.Repositories;

public class InFilePublishRecordRepository : IPublishRecordRepository
{
    private const string FilePath = "publishRecord.json";

    public InFilePublishRecordRepository()
    {
        CreateFileIfNotExist();
    }

    public async Task Save(PublishRecord publishRecord)
    {
        var publishRecords = GetAll();
        publishRecords.Add(publishRecord);
        await using var streamWriter = new StreamWriter(FilePath);
        await streamWriter.WriteAsync(JsonSerializer.Serialize(publishRecords));
    }

    public List<PublishRecord> GetAll()
    {
        using var reader = new StreamReader(FilePath);

        var fileContent = reader.ReadToEnd();
        return JsonSerializer.Deserialize<List<PublishRecord>>(fileContent)!;
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