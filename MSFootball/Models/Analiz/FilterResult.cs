using MSFootball.Models.Data;

namespace MSFootball.Models.Analiz
{
    class FilterResult
    {
        public Team Team { get; set; }

        public FilterResult(Team team)
        {
            Team = team;
        }
    }
}
