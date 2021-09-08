using FootballApp.Infrastructure.Command;
using FootballApp.Infrastructure.Navigators;
using FlashScore.Football;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace FootballApp.ViewModels
{
    class MatchesTableViewModel : ViewModelBase
    {
        readonly ICollectionView collectionView;
        bool selectedAll;
        string searchText;

        public bool SelectedAll
        {
            get => selectedAll;
            set
            {
                if (selectedAll != value)
                {
                    selectedAll = value;
                    foreach (var m in Matches)
                        m.Selected = value;
                }
            }
        }
        public string SearchText
        {
            get => searchText;
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    collectionView.Refresh();
                }
            }
        }
        public MatchViewModel SelectedItem
        {
            get => null;
            set
            {
                value.Selected = !value.Selected;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ReadOnlyObservableCollection<MatchViewModel> Matches { get; }
        public ICommand LoadFullStatCommand { get; }

        public MatchesTableViewModel(INavigator navigator)
        {
            LoadFullStatCommand = new DelegeteCommand(() => navigator.NavigateTo(ViewType.LoaderStatMatches), () => SelectedAll);

            Matches = new ReadOnlyObservableCollection<MatchViewModel>(
                      new ObservableCollection<MatchViewModel>(
                          FootballManager.Matches.Select(m => new MatchViewModel(m, SelectedChangeCallBack))));

            //IEnumerable<Match> matches = new List<Match>();
            //foreach (var m in Matches)
            //{
            //    matches = matches.Concat(m.Match.GetCompletedMatches());
            //}
            //var count = matches.Count();
            //var group = matches.GroupBy(m => m.Url);
            //var countGroup = group.Count();

            collectionView = CollectionViewSource.GetDefaultView(Matches);
            collectionView.Filter = SearchFilter;
        }

        bool SearchFilter(object obj)
        {
            if (string.IsNullOrWhiteSpace(searchText)) return true;
            var match = obj as MatchViewModel;

            if (match.Country.Contains(searchText, StringComparison.OrdinalIgnoreCase)) return true;
            if (match.Championship.Contains(searchText, StringComparison.OrdinalIgnoreCase)) return true;
            if (match.FirstTeamName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) return true;
            if (match.SecondTeamName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }

        void SelectedChangeCallBack(bool selected)
        {
            if (selected == selectedAll) return;
            if (selected)
            {
                Set(ref selectedAll, selected, nameof(SelectedAll));
            }
            else
            {
                foreach (var item in Matches)
                {
                    if (item.Selected) return;
                }
                selectedAll = false;
                OnPropertyChanged(nameof(SelectedAll));
            }
        }
    }
}
