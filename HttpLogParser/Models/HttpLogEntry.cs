namespace HttpLogParser.Models;

public class HttpLogEntry
{
    public string Ip { get; set; }
    public string DateTime { get; set; }
    public string Verb { get; set; }
    public string Uri { get; set; }
    public string Protocol { get; set; }
    public string Status { get; set; }
    public string Length { get; set; }
    public string Referer { get; set; }
    public string UserAgent { get; set; }
}