using FootballApp.Infrastructure.Analiz;
using FootballApp.Infrastructure.Command;
using FootballApp.ViewModels.Filters;
using FlashScore.Football.Analiz;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace FootballApp.ViewModels
{
    class AnalizViewModel : ViewModelBase
    {
        readonly ICollectionView matchesView;

        public ICommand ApplyCommand { get; }
        public ICommand DeleteCommand { get; }

        public FilterGroup FilterGroup { get; }
        public ObservableCollection<IFilter> Filters { get; }
        public ReadOnlyObservableCollection<AnalizTeamViewModel> Teams { get; }
        public FilterInfo SelectedFilter
        {
            get => default;
            set
            {
                Filters.Add(value.Activate());
            }
        }

        public AnalizViewModel(MatchesTableViewModel matchesTable)
        {
            Filters = new ObservableCollection<IFilter>();
            FilterGroup = new PercentFilterGroup(Filters);

            Teams = new ReadOnlyObservableCollection<AnalizTeamViewModel>(new ObservableCollection<AnalizTeamViewModel>(GetTeams(matchesTable.Matches)));

            matchesView = CollectionViewSource.GetDefaultView(Teams);
            matchesView.Filter = ApplyFilter;

            ApplyCommand = new DelegeteCommand(matchesView.Refresh);
            DeleteCommand = new DelegeteCommand<IFilter>(DeleteFilter);
            
            FilterManager.Register("Тотал", () => new TotalViewModel());
            FilterManager.Register("Тотал забитых", () => new TotalCloggedViewModel());
            FilterManager.Register("Тотал пропущенных", () => new TotalMissedViewModel());
            FilterManager.Register("Обе забьют", () => new BothWillScoreViewModel());
            FilterManager.Register("Победа", () => new VictoryViewModel());
            FilterManager.Register("Ничья", () => new DrawMatchViewModel());
            FilterManager.Register("Поражение", () => new DefeatViewModel());
        }

        void DeleteFilter(IFilter filter)
        {
            Filters.Remove(filter);
        }

        bool ApplyFilter(object obj)
        {
            if(obj is AnalizTeamViewModel team)
            {
                var res = FilterGroup.CanFit(team.Team);
                team.FilterResult = res.result;
                return res.isFit;
            }
            return false;
        }

        static List<AnalizTeamViewModel> GetTeams(ICollection<MatchViewModel> matches)
        {
            var res = new List<AnalizTeamViewModel>(matches.Count * 2);
            foreach (var match in matches)
            {
                res.Add(new AnalizTeamViewModel(match.Match, match.Match.FirstTeam));
                res.Add(new AnalizTeamViewModel(match.Match, match.Match.SecondTeam));
            }
            return res;
        }
    }
}
