using DevExpress.Mvvm;
using MSFootball.Models;
using MSFootball.Models.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MSFootball.ViewModels
{
    class TableCurrentMatchesVM : BindableBase
    {
        Window loadManagerWindow;
        bool isCheckedAll;

        public ICommand GetStaticticCommand { get; }
        public ICommand TestCommand { get; }
        public ObservableCollection<CurrentMatch> Matches { get; private set; }
        public bool IsCheckedAll
        {
            get => isCheckedAll;
            set
            {
                if (Matches == null) return;
                isCheckedAll = value;
                foreach (var item in Matches)
                {
                    item.Selected = isCheckedAll;
                }
            }
        }

        public TableCurrentMatchesVM()
        {
            GetStaticticCommand = new DelegateCommand<Window>(GetStatistics, (w) => IsCheckedAll && Matches?.Count > -1 && loadManagerWindow == null);
            ViewTablePage();
        }
        void GetStatistics(Window win)
        {
            if (loadManagerWindow != null) return;

            loadManagerWindow = new LoaderWindow
            {
                Owner = win,
                Topmost = true
            };
            loadManagerWindow.DataContext = new LoaderEndedMatchesVM();

            loadManagerWindow.Closing += (s, e) => { loadManagerWindow = null; };
            loadManagerWindow.Show();
        }
        void ViewTablePage()
        {
            Matches = new ObservableCollection<CurrentMatch>(FootballManager.Matches);
            RaisePropertyChanged("Matches");

            foreach (var item in Matches)
                item.SelectedChanged += CheckedChanged;
        }
        void CheckedChanged(CurrentMatch match)
        {
            if (match.Selected)
            {
                isCheckedAll = true;
                RaisePropertiesChanged("IsCheckedAll");
            }
            else
            {
                foreach (var item in Matches)
                {
                    if (item.Selected) return;
                }
                isCheckedAll = false;
                RaisePropertiesChanged("IsCheckedAll");
            }
        }
    }
}
