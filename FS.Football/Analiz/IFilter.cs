namespace FS.Football.Analiz
{
    public interface IFilter
    {
        bool CanFit(Team team, Match match);
    }
}
