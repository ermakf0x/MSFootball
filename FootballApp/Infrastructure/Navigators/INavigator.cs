using FootballApp.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace FootballApp.Infrastructure.Navigators
{
    enum ViewType : byte
    {
        None = 0,
        MatchesTable,
        Analiz,
        LoaderMatches,
        LoaderStatMatches
    }

    interface INavigator
    {
        event Action NavigationStateChange;
        ViewModelBase CurrentViewModel { get; }
        Visibility VisibilityNavigation { get; }
        ICommand NavigateCommand { get; }
        void NavigateTo(ViewType type);
    }
}
