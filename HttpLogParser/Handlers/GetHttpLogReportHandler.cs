using HttpLogParser.Loaders;
using HttpLogParser.Models;
using HttpLogParser.Parsers;
using HttpLogParser.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace HttpLogParser.Handlers;

public class GetHttpLogReportHandler : IRequestHandler<GetHttpLogReportQuery, HttpLogReport>
{
        readonly ILoader _loader;
        readonly IRepository _repository;
        readonly IParser _parser;
        readonly IOptions<AppOptions> _options;
        readonly ILogger<GetHttpLogReportHandler> _logger;

    public GetHttpLogReportHandler(ILoader loader,  IRepository repository,  IParser parser, IOptions<AppOptions> options,  ILogger<GetHttpLogReportHandler> logger)
    {
        _loader = loader;
        _repository = repository;
        _parser = parser;
        _options = options;
        _logger = logger;
    }

    public async Task<HttpLogReport> Handle(GetHttpLogReportQuery request, CancellationToken cancellationToken)
    {
        var uri = _options.Value.HttpLogUri;

        var lines = _loader.Load(uri);

        foreach (var line in lines)
        {
            var httpLogEntry = _parser.Parse(line);

            if (httpLogEntry != null)
            {
                _repository.AddHttpLogEntry(httpLogEntry);
            }  
        }

        var count = _repository.GetCount;
        var uniqueIps = _repository.GetUniqueIpCount;
        var mostVisitedUrls = _repository.MostVisitedUrls;
        var mostActvieIps = _repository.MostActiveIps;

        return await Task.FromResult(new HttpLogReport
        {
            Count = count,
            UniqueIpAdresses = uniqueIps,
            MostVisitedUrls = mostVisitedUrls,
            MostActiveIps = mostActvieIps
        });
    }
}
