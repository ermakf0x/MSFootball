using FS.Core;
using FS.Football.Extension;

namespace FS.Football.Analiz
{
    public class FTotal : IFilter
    {
        public RefValue<Half> Half { get; }
        public RefValue<LessMoreEqual> LMQ { get; }
        public RefValue<byte> ValueTotal { get; }

        public FTotal()
        {
            Half        = new RefValue<Half>();
            LMQ         = new RefValue<LessMoreEqual>();
            ValueTotal  = new RefValue<byte>();
        }
        public FTotal(RefValue<Half> half, RefValue<LessMoreEqual> lMQ, RefValue<byte> value)
        {
            Half = half ?? throw new System.ArgumentNullException(nameof(half));
            LMQ = lMQ ?? throw new System.ArgumentNullException(nameof(lMQ));
            ValueTotal = value ?? throw new System.ArgumentNullException(nameof(value));
        }

        public bool CanFit(Team team, Match match)
        {
            return match.Total(ValueTotal, LMQ, Half);
        }

        public override string ToString()
        {
            return $"Тотал: {Half}, {LMQ}, {ValueTotal}";
        }
    }
}
