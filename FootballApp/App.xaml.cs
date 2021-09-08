using FootballApp.ViewModels;
using FootballApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FootballApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow() { DataContext = Global.Provider.GetService<MainViewModel>() }.Show();
            base.OnStartup(e);
        }
    }
}
