using FootballApp.Infrastructure.Navigators;
using FootballApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FootballApp
{
    static class Global
    {
        public static int FullStatLimit { get; set; } = 3;
        public static int CompletedMatchesLimit { get; } = 20;
        public static IServiceProvider Provider { get; } = GetServiceProvider();

        static IServiceProvider GetServiceProvider()
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddSingleton<INavigator, Navigator>();
            collection.AddSingleton<MainViewModel>();
            collection.AddSingleton<MatchesTableViewModel>();
            collection.AddSingleton<AnalizViewModel>();
            collection.AddSingleton<LoaderMatchesViewModel>();

            collection.AddTransient<LoaderOfStatForMatchesViewModel>();

            return collection.BuildServiceProvider();
        }
    }
}
