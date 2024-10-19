using HttpLogParser.Models;

namespace HttpLogParser.Repositories;

public interface IRepository
{
    public void AddHttpLogEntry(HttpLogEntry httpLogEntry);
    public int GetCount { get; }
    public int GetUniqueIpCount { get; }
    public IEnumerable<string> MostVisitedUrls { get; }
    public IEnumerable<string> MostActiveIps { get; }
}
