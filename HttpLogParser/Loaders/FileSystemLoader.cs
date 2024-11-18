namespace HttpLogParser.Loaders;

public class FileSystemLoader : ILoader
{
    readonly ILogger<FileSystemLoader> _logger;

    public FileSystemLoader(ILogger<FileSystemLoader> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<string>> Load(string uri, CancellationToken cancellationToken)
    {
        var lines = await File.ReadAllLinesAsync(uri, cancellationToken);

        return lines;
    }
}
