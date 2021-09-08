using DevExpress.Mvvm;
using MSFootball.Models;
using MSFootball.Models.Analiz;
using MSFootball.Models.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MSFootball.ViewModels
{
    class AnalizOfMatchesVM : BindableBase
    {
        List<Team> teamCollection;
        ObservableCollection<Team> _teams;

        public ReadOnlyObservableCollection<Team> Teams { get; private set; }
        public ObservableCollection<FilterBase> UserFilterCollection { get; }

        public FilterBase SelectedUserFilter
        {
            get => selectedUserFilter;
            set
            {
                selectedUserFilter = value;
                RaisePropertyChanged();
            }
        }
        FilterBase selectedUserFilter;

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DeleteAllCommand { get; }
        public ICommand ApplyCommand { get; }

        public AnalizOfMatchesVM()
        {
            UserFilterCollection = new ObservableCollection<FilterBase>();
            AddCommand = new DelegateCommand<FilterRegInfo>(Add);
            DeleteCommand = new DelegateCommand<FilterBase>(Delete);
            DeleteAllCommand = new DelegateCommand(UserFilterCollection.Clear, () => UserFilterCollection.Count > 0);
            ApplyCommand = new DelegateCommand(Apply, () => UserFilterCollection.Count > 0);
        }

        void Apply()
        {
            if (teamCollection == null)
            {
                teamCollection = new List<Team>();
                foreach (var m in FootballManager.Matches)
                {
                    teamCollection.Add(m.FirstTeam);
                    teamCollection.Add(m.SecondTeam);
                }
            }

            if (Teams == null)
            {
                _teams = new ObservableCollection<Team>();
                Teams = new ReadOnlyObservableCollection<Team>(_teams);
                RaisePropertyChanged("Teams");
            }
            else _teams.Clear();

            foreach (var team in teamCollection)
            {
                var isFit = true;

                foreach (var filter in UserFilterCollection)
                {
                    if (!filter.Enabled) continue;
                    if (filter.ApplyFilter(team) is null)
                    {
                        isFit = false;
                        break;
                    }
                }

                if (isFit) _teams.Add(team);
            }
        }
        void Add(FilterRegInfo filterRegInfo)
        {
            UserFilterCollection.Add(FilterManager.CreateInstance(filterRegInfo));
        }
        void Delete(FilterBase filter)
        {
            if (UserFilterCollection.Contains(filter))
            {
                UserFilterCollection.Remove(filter);
            }
        }
    }
}
