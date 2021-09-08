using System;

namespace ConsoleTesting
{
    [Serializable]
    public class ScoreMatch
    {
        readonly Score?[] data;

        public ScoreMatch(Score match)
        {
            data = new Score?[3];
            data[0] = match;
        }
        public ScoreMatch(Score firstHalf, Score SecondHalf) : this(firstHalf + SecondHalf)
        {
            data[1] = firstHalf;
            data[2] = SecondHalf;
        }

        public Score? this[FS.Football.Half half]
        {
            get => data[(byte)half];
            internal set => data[(byte)half] = value;
        }
        public override string ToString() => data[0]?.ToString() ?? "-";
    }

    [Serializable]
    public readonly struct Score
    {
        public readonly byte v1;
        public readonly byte v2;
        public byte Sum => (byte)(v1 + v2);
        public Score(byte value1, byte value2) => (v1, v2) = (value1, value2);
        public static Score operator +(Score a, Score b) => new((byte)(a.v1 + b.v1), (byte)(a.v2 + b.v2));
        public static Score operator -(Score a, Score b) => new((byte)(a.v1 - b.v1), (byte)(a.v2 - b.v2));
        public override string ToString() => $"{v1} : {v2}";
    }
}