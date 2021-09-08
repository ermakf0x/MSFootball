namespace FS.Football.Parsing
{
    public interface IDataParser<T>
    {
        T Parse(string source);
    }
}
