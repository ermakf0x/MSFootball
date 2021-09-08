using FlashScore.Serialization;
using FlashScore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace FlashScore.Football
{
    /// <summary> Статистика за матч </summary>
    [Serializable]
    public class MatchStatistic
    {
        Statistic[] statistics;
        public bool IsEmpty { get; }
        public static MatchStatistic Empty => new();

        public MatchStatistic()
        {
            IsEmpty = true;
        }
        public MatchStatistic(Statistic match, Statistic first, Statistic second)
        {
            if(match == null && first == null && second == null)
            {
                IsEmpty = true;
                return;
            }
            statistics = new Statistic[3];
            statistics[0] = match;
            statistics[1] = first;
            statistics[2] = second;
        }

        public Statistic this[Half half]
        {
            get
            {
                if (IsEmpty) return null;
                return statistics[(byte)half];
            }
        }

        public static MatchStatistic ToObject(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            if (bytes.Length == 1 && bytes[0] == 0) return new MatchStatistic();

            Statistic[] res = new Statistic[3];
            int start = 0;
            int len = bytes[0]; // Длина всего блока byte

            while (len < bytes.Length)
            {
                var s = Statistic.ToObject(bytes, start + 2, len);
                if (s != null && bytes[start + 1] < 3)
                    res[bytes[start + 1]] = s;

                start = len;
                len += bytes[len];
            }

            return new MatchStatistic(res[0], res[1], res[2]);
        }
        public byte[] ToBinary()
        {
            // Если статистика пустая записываем один байт
            if (IsEmpty)
                return new byte[] { 0 };

            List<byte> res = new List<byte>();

            for (byte i = 0; i < 3; i++)
            {
                if (statistics[i] != null)
                {
                    var m = statistics[i].ToBinary();
                    var ml = (byte)(m.Length + 2);
                    res.Add(ml);
                    res.Add(i); // Номер тайма за который статистика
                    res.AddRange(m);
                }
            }
            res.Add((byte)(res.Count + 1)); // байт контроль суммы

            return res.ToArray();
        }
        public override string ToString()
        {
            if (IsEmpty) return "Empty";

            var str = "";
            if (statistics[0] != null)
                str = $"Match: { statistics[0]}; ";
            if (statistics[1] != null)
                str += $"First: {statistics[1]}; ";
            if (statistics[2] != null)
                str += $"Second: {statistics[2]}";

            return str;
        }
    }

    /// <summary> Статистика </summary>
    [Serializable]
    public class Statistic
    {
        const int COUNT = 17; // Количество видов статистик
        IDictionary<StatisticType, StatisticValue> _statistics;

        public Statistic()
        {
            _statistics = new Dictionary<StatisticType, StatisticValue>();
        }

        /// <summary></summary>
        /// <param name="type">Вид статистики</param>
        /// <returns>Статистика</returns>
        public StatisticValue? this[StatisticType type]
        {
            get
            {
                if (_statistics.ContainsKey(type))
                {
                    return _statistics[type];
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    if (_statistics.ContainsKey(type))
                    {
                        _statistics[type] = value.Value;
                    }
                    else
                    {
                        _statistics.Add(type, value.Value);
                    }
                }
            }
        }

        public static Statistic ToObject(byte[] bytes, int start, int length)
        {
            var res = new Statistic();

            for (int i = start; i < length; i += 5)
            {
                if (bytes[i] > COUNT) continue;

                var value = new StatisticValue(BitConverter.ToUInt16(bytes, i + 1), BitConverter.ToUInt16(bytes, i + 3));
                res[(StatisticType)bytes[i]] = value;
            }

            return res;
        }
        public byte[] ToBinary()
        {
            var res = new List<byte>(_statistics.Count * 5);

            foreach (var stat in _statistics)
            {
                res.Add((byte)stat.Key);
                res.AddRange(stat.Value.ToBinary());
            }

            return res.ToArray();
        }
        public override string ToString()
        {
            return _statistics.Count.ToString();
        }
    }

    [Serializable]
    public struct StatisticValue : ISerializable
    {
        public ushort Value { get; }
        public ushort Value2 { get; }
        public int Sum => Value + Value2;

        public StatisticValue(ushort value, ushort value2)
        {
            Value = value;
            Value2 = value2;
        }

        public static StatisticValue operator +(StatisticValue v1, StatisticValue v2)
        {
            return new StatisticValue((ushort)(v1.Value + v2.Value), (ushort)(v1.Value2 + v2.Value2));
        }

        public byte[] ToBinary()
        {
            var res = new byte[4];
            var v = BitConverter.GetBytes(Value);
            var v2 = BitConverter.GetBytes(Value2);
            res[0] = v[0];
            res[1] = v[1];
            res[2] = v2[0];
            res[3] = v2[1];
            return res;
        }
        public override string ToString() => $"({Value}:{Value2})";

        #region Serializable

        StatisticValue(SerializationInfo info, StreamingContext context)
        {
            SerializationReader r = SerializationReader.GetReader(info);
            Value = r.ReadByte();
            Value2 = r.ReadByte();
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter w = SerializationWriter.GetWriter();
            w.Write(Value);
            w.Write(Value2);
            w.AddToInfo(info);
        }

        #endregion
    }

    /// <summary> Тип статистики </summary>
    [Serializable]
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum StatisticType : byte
    {
        /// <summary> Владение мячом </summary>
        [Description("Владение мячом")]
        BallPossession,
        /// <summary> Удары </summary>
        [Description("Удары")]
        GoalAttempts,
        /// <summary> Удары в створ </summary>
        [Description("Удары в створ")]
        ShotsOnGoal,
        /// <summary> Удары мимо </summary>
        [Description("Удары мимо")]
        ShotsOffGoal,
        /// <summary> Блоковано ударов </summary>
        [Description("Блоковано ударов")]
        BlockedStrokes,
        /// <summary> Штрафные </summary>
        [Description("Штрафные")]
        FreeKicks,
        /// <summary> Угловые </summary>
        [Description("Угловые")]
        CornerKicks,
        /// <summary> Офсайды </summary>
        [Description("Офсайды")]
        Offside,
        /// <summary> Вбрасывания </summary>
        [Description("Вбрасывания")]
        ThrowIn,
        /// <summary> Сэйвы </summary>
        [Description("Сэйвы")]
        Saves,
        /// <summary> Фолы </summary>
        [Description("Фолы")]
        Fouls,
        /// <summary> Желтые карточки </summary>
        [Description("Желтые карточки")]
        YellowCards,
        /// <summary> Красные карточки </summary>
        [Description("Красные карточки")]
        RedCards,
        /// <summary> Всего передач </summary>
        [Description("Всего передач")]
        TotalPasses,
        /// <summary> Oтборы </summary>
        [Description("Oтборы")]
        Selections,
        /// <summary> Атаки </summary>
        [Description("Атаки")]
        Attacks,
        /// <summary> Опасные атаки </summary>
        [Description("Опасные атаки")]
        DangerousAttacks
    }
}
