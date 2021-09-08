using FlashScore.Football;
using FlashScore.Football.Analiz;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballApp.Infrastructure.Analiz
{
    public class PercentFilterGroup : FilterGroup
    {
        public override string ColumnName => "%";

        public PercentFilterGroup(IList<IFilter> filters) : base(filters) { }

        public override (bool isFit, object result) CanFit(Team team)
        {
            if (Filters.Count == 0) return (true, null);

            //var res = new List<float>(Filters.Count);
            //var max = 0f;
            var min = 100f;

            foreach (var f in Filters)
            {
                var res2 = CanFit(team, (m) => f.CanFit(team, m));
                if (!res2.isFit) return (false, null);

                var value = (float)res2.result;
                //res.Add(value);
                //max = Math.Max(max, value);
                min = Math.Min(min, value);
            }
            //var resStr = $"min:{min}; max:{max}; ave:{res.Average()}";
            return (true, (int)min);
        }

        protected override (bool isFit, object result) CanFit(Team team, Predicate<Match> canFit)
        {
            if (team.LastMatches == null || team.LastMatches.Count == 0) return (false, null);

            var matches = team.LastMatches;
            var count = Global.FullStatLimit < matches.Count ? Global.FullStatLimit : matches.Count;
            float countOfFit = 0;

            for (int i = 0; i < count; i++)
                if (canFit(matches[i])) countOfFit++;

            if (countOfFit == 0) return (false, null);

            var percent = countOfFit / count * 100; // считаем проценты
            return (percent >= 50, percent);
        }
    }
}
