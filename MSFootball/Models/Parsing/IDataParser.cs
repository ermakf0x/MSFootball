namespace MSFootball.Models.Parsing
{
    interface IDataParser<T>
    {
        T Parse(string source);
    }
}
