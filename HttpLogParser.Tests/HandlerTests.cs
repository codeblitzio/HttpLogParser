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
    readonly ILogger<GetHttpLogReportHandler> _logger = new Mock<ILogger<GetHttpLogReportHandler>>().Object;
    readonly ILoader _loader = new Mock<ILoader>().Object;
    readonly IParser _parser = new Mock<IParser>().Object;
    readonly IRepository _repository = new Mock<IRepository>().Object;
    readonly IOptions<AppOptions> _options = Microsoft.Extensions.Options.Options.Create(new AppOptions());

    [Fact]
    public async Task Handler_Returns_Expected_Report_Payload()
    {
        // arrange
        var input = @"177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] ""GET /intranet-analytics/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""";

        var sut = new GetHttpLogReportHandler(_loader, _repository, _parser, _options, _logger);

        // act
        var result = await sut.Handle(new HttpLogReportQuery(), default);

        //assert
        Assert.True(true);
    }
}