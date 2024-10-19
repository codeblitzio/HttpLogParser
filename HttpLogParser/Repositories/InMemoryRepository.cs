using HttpLogParser.Models;

namespace HttpLogParser.Repositories;

public class InMemoryRepository : IRepository
{
    private List<HttpLogEntry> _httpLogEntries = [];

    public int GetCount => _httpLogEntries.Count();

    public int GetUniqueIpCount => _httpLogEntries.GroupBy(x => x.Ip).Count();

    public IEnumerable<string> MostVisitedUrls
    {
        get
        {
            var urlGroups = _httpLogEntries.GroupBy(x => x.Uri).OrderByDescending(x => x.Count()).Take(3);
            var urls = urlGroups.Select(x => x.Key);
            return urls;
        }
    } 

    public IEnumerable<string> MostActiveIps
    {
        get
        {
            var ipGroups = _httpLogEntries.GroupBy(x => x.Ip).OrderByDescending(x => x.Count()).Take(3);
            var ips = ipGroups.Select(x => x.Key);
            return ips;
        }
    }

    public void AddHttpLogEntry(HttpLogEntry httpLogEntry)
    {
        _httpLogEntries.Add(httpLogEntry);
    }
}
