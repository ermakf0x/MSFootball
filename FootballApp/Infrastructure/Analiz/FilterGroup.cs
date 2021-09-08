using FlashScore.Football;
using FlashScore.Football.Analiz;
using System;
using System.Collections.Generic;

namespace FootballApp.Infrastructure.Analiz
{
    public abstract class FilterGroup
    {
        public IList<IFilter> Filters { get; }
        public abstract string ColumnName { get; }

        public FilterGroup(IList<IFilter> filters)
        {
            Filters = filters;
        }

        public virtual (bool isFit, object result) CanFit(Team team)
        {
            foreach (var f in Filters)
            {
                if (!CanFit(team, (m) => f.CanFit(team, m)).isFit)
                    return (false, null);
            }
            return (true, null);
        }
        protected abstract (bool isFit, object result) CanFit(Team team, Predicate<Match> canFit);
    }
}
