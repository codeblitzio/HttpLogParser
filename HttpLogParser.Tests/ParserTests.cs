using HttpLogParser.Parsers;
using Microsoft.Extensions.Logging;
using Moq;

namespace HttpLogParser.Tests;

public class ParserTests
{
    readonly ILogger<RegexParser> _logger = new Mock<ILogger<RegexParser>>().Object;

    [Fact]
    public void Regex_Matches_A_Valid_Log_Entry()
    {
        // arrange
        var input = @"177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] ""GET /intranet-analytics/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""";

        var sut = new RegexParser(_logger);

        // act
        var result = sut.Parse(input);

        // assert
        Assert.NotNull(result);
        Assert.Equal("177.71.128.21", result.Ip);
        Assert.Equal("10/Jul/2018:22:21:28 +0200", result.DateTime);
        Assert.Equal("GET", result.Verb);
        Assert.Equal("/intranet-analytics/", result.Uri);
        Assert.Equal("200", result.Status);
        Assert.Equal("3574", result.Length);
        Assert.Equal("HTTP/1.1", result.Protocol);
        Assert.Equal("-", result.Referer);
        Assert.Equal("Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7", result.UserAgent);
    }

    [Fact]
    public void Regex_Does_Not_Match_An_Invalid_Log_Entry()
    {
         // arrange
        var input = @"177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] ""GET /intranet-analytics/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7"" extra";

        var parser = new RegexParser(_logger);

        // act
        var result = parser.Parse(input);

        // assert
        Assert.Null(result);
    }
}