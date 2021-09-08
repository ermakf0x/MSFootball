using FlashScore.Football.Analiz;
using System;
using System.Collections.ObjectModel;

namespace FootballApp.Infrastructure.Analiz
{
    static class FilterManager
    {
        static readonly ObservableCollection<FilterInfo> filters = new();
        public static ReadOnlyObservableCollection<FilterInfo> Filters => new(filters);

        public static void Register<T>(string name, Func<T> activate) where T : IFilter
        {
            filters.Add(new FilterInfo
            {
                Name = name,
                Activate = activate as Func<IFilter>
            });
        }
    }

    struct FilterInfo
    {
        public string Name;
        public Func<IFilter> Activate;
        public override string ToString() => Name;
    }
}
