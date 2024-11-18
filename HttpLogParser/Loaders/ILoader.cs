namespace HttpLogParser.Loaders;

public interface ILoader
{
    public Task<IEnumerable<string>> Load(string uri, CancellationToken cancellationToken);
}
