using System.Text.RegularExpressions;
using HttpLogParser.Models;

namespace HttpLogParser.Parsers;

public class RegexParser : IParser
{
    public HttpLogEntry Parse(string input)
    {
        HttpLogEntry result = null;

        var pattern = """^(\S*).*\[(.*)\]\s"(\S*)\s(\S*)\s([^"]*)"\s(\S*)\s(\S*)\s"([^"]*)"\s"([^"]*)"$""";

        if (Regex.IsMatch(input, pattern))
        {
            result = new HttpLogEntry();

            foreach (Match match in Regex.Matches(input, pattern))
            {
                result.Ip = match.Groups[1].Value;
                result.DateTime = match.Groups[2].Value;
                result.Verb = match.Groups[3].Value;
                result.Uri = match.Groups[4].Value;
                result.Protocol = match.Groups[5].Value;
                result.Status = match.Groups[6].Value;
                result.Length = match.Groups[7].Value;
                result.Referer = match.Groups[8].Value;
                result.UserAgent = match.Groups[9].Value;
            }
        }

        return result;
    }
}
