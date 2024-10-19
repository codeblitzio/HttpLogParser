using MediatR;

namespace HttpLogParser.Models;

public class GetHttpLogReportQuery : IRequest<HttpLogReport>
{
}
