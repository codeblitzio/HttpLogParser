using HttpLogParser.Models;

namespace HttpLogParser.Parsers;

public interface IParser
{
    public HttpLogEntry Parse(string input);
}
