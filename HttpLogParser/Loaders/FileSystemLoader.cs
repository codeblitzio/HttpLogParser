namespace HttpLogParser.Loaders;

public class FileSystemLoader : ILoader
{
    readonly ILogger<FileSystemLoader> _logger;

    public FileSystemLoader(ILogger<FileSystemLoader> logger)
    {
        _logger = logger;
    }

    public IEnumerable<string> Load(string uri)
    {
        var lines = File.ReadAllLines(uri);

        return lines;
    }
}
