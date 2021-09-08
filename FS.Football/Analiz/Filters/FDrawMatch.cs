using FS.Core;
using FS.Football.Extension;
using System;

namespace FS.Football.Analiz
{
    /// <summary> Ничья </summary>
    public class FDrawMatch : IFilter
    {
        public RefValue<Half> Half { get; }

        public FDrawMatch()
        {
            Half = new RefValue<Half>();
        }
        public FDrawMatch(RefValue<Half> half)
        {
            Half = half ?? throw new ArgumentNullException(nameof(half));
        }

        public bool CanFit(Team team, Match match)
        {
            return match.Draw(Half);
        }

        public override string ToString()
        {
            return $"Ничья: {Half}";
        }
    }
}
