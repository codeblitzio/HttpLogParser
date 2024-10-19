namespace HttpLogParser.Models;

public class HttpLogReport
{
    public int UniqueIpAdresses { get; set; }

    public IEnumerable<string> MostVisitedUrls {get; set; }
    
    public IEnumerable<string> MostActiveIps {get; set; }
}
