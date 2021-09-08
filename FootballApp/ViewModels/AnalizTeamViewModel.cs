using FootballApp.Infrastructure;
using FlashScore.Football;

namespace FootballApp.ViewModels
{
    class AnalizTeamViewModel : ObservableObject
    {
        public object FilterResult { get; set; }
        public Match Current { get; }
        public Team Team { get; }
        public string Name => Team?.Name;

        public AnalizTeamViewModel(Match current, Team team)
        {
            Current = current;
            Team = team;
        }
        public override string ToString() => Name;
    }
}
