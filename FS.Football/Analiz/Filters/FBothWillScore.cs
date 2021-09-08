using FS.Core;
using FS.Football.Extension;
using System;

namespace FS.Football.Analiz
{
    /// <summary>Оба забьют</summary>
    public class FBothWillScore : IFilter
    {
        public RefValue<Half> Half { get; }

        public FBothWillScore()
        {
            Half = new RefValue<Half>();
        }
        public FBothWillScore(RefValue<Half> half)
        {
            Half = half ?? throw new ArgumentNullException(nameof(half));
        }

        public bool CanFit(Team team, Match match)
        {
            return match.BothWillScore(Half);
        }

        public override string ToString()
        {
            return $"Обе команды забъют: {Half}";
        }
    }
}
