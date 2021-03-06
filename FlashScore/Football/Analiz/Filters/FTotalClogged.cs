using FlashScore.Football.Utils;
using System;

namespace FlashScore.Football.Analiz
{
    public class FTotalClogged : IFilter
    {
        public RefValue<Half> Half { get; }
        public RefValue<LessMoreEqual> LMQ { get; }
        public RefValue<byte> ValueTotal { get; }

        public FTotalClogged()
        {
            Half        = new RefValue<Half>();
            LMQ         = new RefValue<LessMoreEqual>();
            ValueTotal  = new RefValue<byte>();
        }
        public FTotalClogged(RefValue<Half> half, RefValue<LessMoreEqual> lMQ, RefValue<byte> valueTotal)
        {
            Half        = half ?? throw new ArgumentNullException(nameof(half));
            LMQ         = lMQ ?? throw new ArgumentNullException(nameof(lMQ));
            ValueTotal  = valueTotal ?? throw new ArgumentNullException(nameof(valueTotal));
        }

        public bool CanFit(Team team, Match match)
        {
            return match.TotalClogged(team, ValueTotal, LMQ, Half);
        }

        public override string ToString()
        {
            return $"Тотал забитых: {Half}, {LMQ}, {ValueTotal}";
        }
    }
}
