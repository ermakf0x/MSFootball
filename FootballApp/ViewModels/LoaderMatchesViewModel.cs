using FootballApp.Infrastructure.Command;
using FootballApp.Infrastructure.Logging;
using FootballApp.Infrastructure.Navigators;
using FlashScore.Football;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FootballApp.ViewModels
{
    class LoaderMatchesViewModel : ViewModelBase
    {
        readonly INavigator navigator;
        string exceptionMessage;
        Visibility exceptionVisibility;

        public string ExceptionMessage { get => exceptionMessage; private set => Set(ref exceptionMessage, value); }
        public Visibility ExceptionVisibility { get => exceptionVisibility; private set => Set(ref exceptionVisibility, value); }
        public ICommand LoadMatchesCommand { get; }

        public LoaderMatchesViewModel(INavigator navigator)
        {
            this.navigator = navigator;
            LoadMatchesCommand = new AsyncDelegeteCommand(LoadMatchesAsync);
            
            _ = LoadMatchesAsync();
        }

        async Task LoadMatchesAsync()
        {
            ExceptionVisibility = Visibility.Hidden;
            ExceptionMessage = "";

            try
            {
                await FootballManager.GetMatchesAsync(default);
                navigator.NavigateTo(ViewType.MatchesTable);
            }
            catch (HttpRequestException)
            {
                ExceptionMessage = "Ошибка загрузки данных";
                ExceptionVisibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Logger.Send(ex.Message, Logger.MessageType.ERROR);
                ExceptionMessage = "ERROR: " + ex.Message;
                ExceptionVisibility = Visibility.Visible;
            }
        }
    }
}
