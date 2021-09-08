using FootballApp.Infrastructure;
using FlashScore.Football;
using System;

namespace FootballApp.ViewModels
{
    class MatchViewModel : ObservableObject
    {
        bool selected;
        readonly Action<bool> selectedChange;

        public Match Match { get; }
        public DateTime Time => Match.Time;
        public string Country => Match.Country;
        public string Championship => Match.Championship;
        public string FirstTeamName => Match.FirstTeam.Name;
        public string SecondTeamName => Match.SecondTeam.Name;
        public bool Selected
        {
            get => selected;
            set
            {
                if (Set(ref selected, value))
                    selectedChange(selected);
            }
        }

        public MatchViewModel(Match match, Action<bool> selectedChange)
        {
            Match = match;
            this.selectedChange = selectedChange;
        }
    }
}
