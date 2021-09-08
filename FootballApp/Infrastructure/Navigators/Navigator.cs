using FootballApp.Infrastructure.Command;
using FootballApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Input;

namespace FootballApp.Infrastructure.Navigators
{
    class Navigator : INavigator
    {
        ViewType lastViewType;

        public event Action NavigationStateChange;
        public ViewModelBase CurrentViewModel { get; private set; }
        public Visibility VisibilityNavigation { get; private set; }
        public ICommand NavigateCommand { get; }

        public Navigator()
        {
            NavigateCommand = new DelegeteCommand<ViewType>(NavigateTo);
        }

        public void NavigateTo(ViewType type)
        {
            if (lastViewType == type) return;

            switch (type)
            {
                case ViewType.MatchesTable:
                    SetViewModel(Global.Provider.GetService<MatchesTableViewModel>());
                    break;
                case ViewType.Analiz:
                    SetViewModel(Global.Provider.GetService<AnalizViewModel>());
                    break;
                case ViewType.LoaderMatches:
                    SetViewModel(Global.Provider.GetService<LoaderMatchesViewModel>(), false);
                    break;
                case ViewType.LoaderStatMatches:
                    SetViewModel(Global.Provider.GetService<LoaderOfStatForMatchesViewModel>(), false);
                    break;
                default:
                    break;
            }

            lastViewType = type;
            NavigationStateChange?.Invoke();
        }
        void SetViewModel(ViewModelBase vm, bool visibilityNavigation = true)
        {
            VisibilityNavigation = visibilityNavigation ? Visibility.Visible : Visibility.Collapsed;
            CurrentViewModel = vm;
        }
    }
}
