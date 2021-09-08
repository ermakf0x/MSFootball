using System;

namespace MSFootball.Models.Data
{
    /// <summary>
    /// Игровой счет за матч
    /// </summary>
    [Serializable]
    public class ScoreMatch
    {
        readonly Score[] score;

        public ScoreMatch(Score match)
        {
            score = new Score[3];
            score[0] = match;
        }

        public Score? this[Half half]
        {
            get => score[(byte)half];
        }
        public void AddFirst(Score first)
        {
            score[1] = first;
        }
        public void AddSecond(Score second)
        {
            score[2] = second;
        }
        public short ToBinary()
        {
            return BitConverter.ToInt16(new[] { score[0].First, score[0].Second }, 0);
        }
        public static ScoreMatch ToObject(short s)
        {
            var b = BitConverter.GetBytes(s);
            return new ScoreMatch(new Score(b[0], b[1]));
        }
        public override string ToString()
        {
            return score[0].ToString();
        }

        public static Score? ParseScore(string scoreStr)
        {
            if (scoreStr.Contains("-")) return null;

            if (scoreStr.Contains("(")) // "3 : 2(2 : 2)"
            {
                var pS = _ParseScore(scoreStr.Split('(')[0]);
                return new Score(pS[0], pS[1]);
            }
            else // "2 : 1"
            {
                var pS = _ParseScore(scoreStr);
                return new Score(pS[0], pS[1]);
            }
        }
        static byte[] _ParseScore(string score)
        {
            byte[] res = new byte[2];

            var a = score.Split(':');

            res[0] = byte.Parse(a[0]);
            res[1] = byte.Parse(a[1]);

            return res;
        }
    }

    /// <summary>
    /// Счет
    /// </summary>
    [Serializable]
    public struct Score
    {
        public byte First { get; set; }
        public byte Second { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first">Счет первой команды</param>
        /// <param name="second">Счет второй команды</param>
        public Score(byte first, byte second)
        {
            First = first;
            Second = second;
        }

        public static Score operator -(Score a, Score b)
        {
            return new Score((byte)(a.First - b.First), (byte)(a.Second - b.Second));
        }
        public static Score operator +(Score a, Score b)
        {
            return new Score((byte)(a.First + b.First), (byte)(a.Second + b.Second));
        }

        public override string ToString()
        {
            return $"({First}:{Second})";
        }
    }
}
