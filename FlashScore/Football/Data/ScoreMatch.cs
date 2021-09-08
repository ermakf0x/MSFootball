using FlashScore.Serialization;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FlashScore.Football
{
    /// <summary> Игровой счет за матч </summary>
    [Serializable]
    public class ScoreMatch : ISerializable
    {
        Score?[] score;

        public bool IsEmpty { get; private set; }
        public Score? Match => IsEmpty ? null : score[0];
        public Score? FirstHalf => IsEmpty ? null : score[1];
        public Score? SecondHalf => IsEmpty ? null : score[2];

        public static ScoreMatch Empty => new ScoreMatch();

        public ScoreMatch() { IsEmpty = true; }
        public ScoreMatch(Score match)
        {
            score = new Score?[3];
            score[0] = match;
        }
        public ScoreMatch(Score first, Score second)
        {
            score = new Score?[3];
            score[0] = first + second;
            score[1] = first;
            score[2] = second;
        }

        public Score? this[Half half]
        {
            get
            {
                if (IsEmpty) return null;
                return score[(byte)half];
            }
        }

        public ScoreMatch SetFirst(byte fValue, byte sValue) => SetFirst(new Score(fValue, sValue));
        public ScoreMatch SetFirst(Score value)
        {
            if (IsEmpty)
            {
                score = new Score?[3];
                IsEmpty = false;
            }

            score[1] = value;

            if (!score[0].HasValue && score[2].HasValue)
            {
                score[0] = score[1] + score[2];
            }

            return this;
        }
        public ScoreMatch SetSecond(byte fValue, byte sValue) => SetSecond(new Score(fValue, sValue));
        public ScoreMatch SetSecond(Score value)
        {
            if (IsEmpty)
            {
                score = new Score?[3];
                IsEmpty = false;
            }

            score[2] = value;

            if (!score[0].HasValue && score[1].HasValue)
            {
                score[0] = score[1] + score[2];
            }

            return this;
        }

        public byte[] ToBinary()
        {
            if (IsEmpty) return new byte[] { 0 };
            return new[] { score[0].Value.First, score[0].Value.Second };
        }
        public static ScoreMatch ToObject(byte[] bytes)
        {
            if (bytes?.Length >= 2) new ScoreMatch(new Score(bytes[0], bytes[1]));
            return new ScoreMatch();
        }
        public override string ToString()
        {
            return IsEmpty ? "Empty" : score[0].ToString();
        }

        public static ScoreMatch Parse(string scoreStr)
        {
            if (scoreStr.Contains("-")) return new ScoreMatch();

            if (scoreStr.Contains("(")) // "3 : 2(2 : 2)"
            {
                var pS = ParseScore(scoreStr.Split('(')[0]);
                return new ScoreMatch(new Score(pS[0], pS[1]));
            }
            else // "2 : 1"
            {
                var pS = ParseScore(scoreStr);
                return new ScoreMatch(new Score(pS[0], pS[1]));
            }
        }
        static byte[] ParseScore(string score)
        {
            byte[] res = new byte[2];

            var a = score.Split(':');

            res[0] = byte.Parse(a[0]);
            res[1] = byte.Parse(a[1]);

            return res;
        }

        internal void UpdateScore(MatchSummary summary)
        {
            if (summary[Half.First] != null)
            {
                byte s1 = 0, s2 = 0;
                foreach (var me in summary[Half.First])
                {
                    if (me.Type == MatchEventType.Goal || me.Type == MatchEventType.AutoGoal)
                    {
                        if (me.TeamID == 1) s1++;
                        else s2++;
                    }
                }
                SetFirst(new(s1, s2));
            }

            if (summary[Half.Second] != null)
            {
                byte s1 = 0, s2 = 0;
                foreach (var a in summary[Half.Second])
                {
                    if (a.Type == MatchEventType.Goal || a.Type == MatchEventType.AutoGoal)
                    {
                        if (a.TeamID == 1) s1++;
                        else s2++;
                    }
                }
                SetSecond(new(s1, s2));
            }
        }


        #region Serializable

        ScoreMatch(SerializationInfo info, StreamingContext context)
        {
            SerializationReader r = SerializationReader.GetReader(info);
            var list = r.ReadList<Score?>();
            if (list != null) score = list.ToArray();
            else IsEmpty = true;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter w = SerializationWriter.GetWriter();
            w.Write(score);
            w.AddToInfo(info);
        }

        #endregion
    }

    /// <summary> Счет </summary>
    [Serializable]
    public struct Score : ISerializable
    {
        public byte First { get; set; }
        public byte Second { get; set; }
        public int Sum => First + Second;

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
            return $"{First}:{Second}";
        }

        #region Serializable

        Score(SerializationInfo info, StreamingContext context)
        {
            SerializationReader r = SerializationReader.GetReader(info);
            First = r.ReadByte();
            Second = r.ReadByte();
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter w = SerializationWriter.GetWriter();
            w.Write(First);
            w.Write(Second);
            w.AddToInfo(info);
        }

        #endregion
    }
}
