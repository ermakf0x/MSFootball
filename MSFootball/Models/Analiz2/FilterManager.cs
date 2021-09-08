using MSFootball.Models.Analiz;
using MSFootball.Models.Data;
using System.Collections.Generic;

namespace MSFootball.Models.Analiz2
{
    public static class FilterManager
    {
        public static List<ICriterion> Filters { get; set; }


    }

    public class Total : ICriterion
    {
        private readonly Value<Half> half;
        private readonly Value<int> total;
        private readonly Value<LessMoreEqual> lessMoreEqual;

        public Total(Value<Half> half, Value<int> total, Value<LessMoreEqual> lessMoreEqual)
        {
            this.half = half;
            this.total = total;
            this.lessMoreEqual = lessMoreEqual;
        }
        public bool CanFit(EndedMatch match)
        {
            var score = match.Score[half];
            if (score.HasValue)
            {
                var totalValue = score.Value.First + score.Value.Second;

                switch ((LessMoreEqual)lessMoreEqual)
                {
                    case LessMoreEqual.More: return totalValue > total;
                    case LessMoreEqual.Less: return totalValue < total;
                    case LessMoreEqual.Equal: return totalValue == total;
                }
            }

            return false;
        }
    }
}
