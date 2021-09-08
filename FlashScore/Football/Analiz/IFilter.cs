namespace FlashScore.Football.Analiz
{
    public interface IFilter
    {
        bool CanFit(Team team, Match match);
    }
}
