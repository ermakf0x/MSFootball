namespace FlashScore.Football.Parsing
{
    public interface IDataParser<T>
    {
        T Parse(string source);
    }
}
