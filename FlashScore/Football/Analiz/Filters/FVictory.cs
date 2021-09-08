using FlashScore.Football.Utils;
using System;

namespace FlashScore.Football.Analiz
{
    /// <summary> Победа </summary>
    public class FVictory : IFilter
    {
        public RefValue<Half> Half { get; }

        public FVictory()
        {
            Half = new RefValue<Half>();
        }
        public FVictory(RefValue<Half> half)
        {
            Half = half ?? throw new ArgumentNullException(nameof(half));
        }

        public bool CanFit(Team team, Match match)
        {
            return match.Victory(team, Half);
        }

        public override string ToString()
        {
            return $"Победа: {Half}";
        }
    }
}
