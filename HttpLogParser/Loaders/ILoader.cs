namespace HttpLogParser.Loaders;

public interface ILoader
{
    public IEnumerable<string> Load(string uri);
}
