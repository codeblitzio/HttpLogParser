using HttpLogParser.Handlers;
using HttpLogParser.Loaders;
using HttpLogParser.Models;
using HttpLogParser.Parsers;
using HttpLogParser.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace HttpLogParser.Tests;

public class HandlerTests
{
    readonly Mock<ILoader> _loader;
    readonly Mock<ILogger<GetHttpLogReportHandler>> _logger;
    readonly IOptions<AppOptions> _options;
    readonly IParser _parser;
    readonly IRepository _repository;

    public HandlerTests()
    {
       // we'll mock the loader as this has an external dependency on the file system
       _loader = new Mock<ILoader>();

        // we'll mock the logger
        _logger = new Mock<ILogger<GetHttpLogReportHandler>>();

        // we'll mock the app options
        _options = Options.Create(new AppOptions {  HttpLogUri = "" });

        // let's not mock the parser or repository as these have no external dependencies
        _parser = new RegexParser(new Mock<ILogger<RegexParser>>().Object);
        _repository = new InMemoryRepository(new Mock<ILogger<InMemoryRepository>>().Object);
    }

    [Fact]
    public async Task Handler_Returns_Expected_Report_Payload()
    {
        // arrange
        var data = new List<string>
        {
            @"177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] ""GET /index/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""",
            @"177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] ""GET /index/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""",
            @"177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] ""GET /index/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""", 
            @"177.71.128.22 - - [10/Jul/2018:22:21:28 +0200] ""GET /about/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""",
            @"177.71.128.22 - - [10/Jul/2018:22:21:28 +0200] ""GET /about/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""", 
            @"177.71.128.23 - - [10/Jul/2018:22:21:28 +0200] ""GET /home/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""",
            @"177.71.128.23 - - [10/Jul/2018:22:21:28 +0200] ""GET /home/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""", 
            @"177.71.128.24 - - [10/Jul/2018:22:21:28 +0200] ""GET /help/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7"""  
        };

        _loader.Setup(_ => _.Load(It.IsAny<string>()))
            .Returns(data);

        var sut = new GetHttpLogReportHandler(_loader.Object, _repository, _parser, _options, _logger.Object);

        // act
        var result = await sut.Handle(new HttpLogReportQuery(), default);

        //assert
        Assert.Equal(4, result.UniqueIpAdresses);
        Assert.Equal("177.71.128.21", result.MostActiveIps.ToArray()[0]);
        Assert.Equal("177.71.128.22", result.MostActiveIps.ToArray()[1]);
        Assert.Equal("177.71.128.23", result.MostActiveIps.ToArray()[2]);
        Assert.Equal("/index/", result.MostVisitedUrls.ToArray()[0]);
        Assert.Equal("/about/", result.MostVisitedUrls.ToArray()[1]);
        Assert.Equal("/home/", result.MostVisitedUrls.ToArray()[2]); 
    }
}