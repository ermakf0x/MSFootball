using FootballApp.Infrastructure;
using FlashScore.Football;
using FlashScore.Football.Analiz;

namespace FootballApp.ViewModels.Filters
{
    abstract class FilterViewModel<TFilter> : ObservableObject, IFilter
        where TFilter: IFilter, new()
    {
        protected readonly TFilter filter = new();
        public abstract string Name { get; }
        public bool CanFit(Team team, Match match) => filter.CanFit(team, match);
    }
}
