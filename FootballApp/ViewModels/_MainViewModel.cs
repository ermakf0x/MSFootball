using FootballApp.Infrastructure.Navigators;
using System.Windows;
using System.Windows.Input;

namespace FootballApp.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        readonly INavigator _navigate;

        public ViewModelBase CurrentViewModel => _navigate.CurrentViewModel;
        public ICommand NavigateCommand => _navigate.NavigateCommand;
        public Visibility VisibilityNavigation => _navigate.VisibilityNavigation;

        public MainViewModel(INavigator navigate)
        {
            _navigate = navigate;
            _navigate.NavigationStateChange += () =>
            {
                OnPropertyChanged(nameof(CurrentViewModel));
                OnPropertyChanged(nameof(VisibilityNavigation));
            };
            _navigate.NavigateTo(ViewType.LoaderMatches);
            //_navigate.NavigateTo(ViewType.Analiz);
        }
    }
}
