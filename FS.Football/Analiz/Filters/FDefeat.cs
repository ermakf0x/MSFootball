using FS.Core;
using FS.Football.Extension;
using System;

namespace FS.Football.Analiz
{
    /// <summary> Поражение </summary>
    public class FDefeat : IFilter
    {
        public RefValue<Half> Half { get; }

        public FDefeat()
        {
            Half = new RefValue<Half>();
        }
        public FDefeat(RefValue<Half> half)
        {
            Half = half ?? throw new ArgumentNullException(nameof(half));
        }

        public bool CanFit(Team team, Match match)
        {
            return match.Defeat(team, Half);
        }

        public override string ToString()
        {
            return $"Поражение: {Half}";
        }
    }
}
