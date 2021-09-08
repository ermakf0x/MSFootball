using FlashScore.Serialization;
using FlashScore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FlashScore.Football
{
    /// <summary> Обзор игровых событий </summary>
    [Serializable]
    public class MatchSummary
    {
        readonly Summary[] summary;
        public bool IsEmpty { get; }
        public static MatchSummary Empty = new MatchSummary();

        public MatchSummary(){ IsEmpty = true; }
        public MatchSummary(Summary first, Summary second)
        {
            if (first is null && second is null)
            {
                IsEmpty = true;
                return;
            }
            summary = new Summary[3];
            summary[1] = first;
            summary[2] = second;
        }

        public Summary this[Half half]
        {
            get
            {
                return (half, IsEmpty) switch
                {
                    (_, true)           => null,
                    (Half.Match, _)     => GetSummaryAllMatch(),
                    (Half.First, _)     => summary[1],
                    (Half.Second, _)    => summary[2],
                    _                   => null
                };
            }
        }

        Summary GetSummaryAllMatch()
        {
            if (summary[0] != null) return summary[0];

            if (summary[1] == null || summary[2] == null)
            {
                if (summary[1] == null)
                {
                    summary[0] = summary[2];
                }
                else
                {
                    summary[0] = summary[1];
                }
            }
            else
            {
                summary[0] = summary[1] + summary[2];
            }
            return summary[0];
        }

        public static MatchSummary ToObject(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            if (bytes.Length == 1 && bytes[0] == 0)
                return new MatchSummary();

            var res = new Summary[2];

            var indent = BitConverter.ToUInt16(bytes, 0);
            var s = Summary.ToObject(bytes, 3, indent);
            if (s != null && bytes[2] <= 2)
            {
                res[bytes[2]] = s;
            }

            if (indent < bytes.Length)
            {
                s = Summary.ToObject(bytes, indent + 3, bytes.Length);
                if (s != null && bytes[indent + 2] <= 2)
                {
                    res[bytes[indent + 2]] = s;
                }
            }

            return new MatchSummary(res[0], res[1]);
        }
        public byte[] ToBinary()
        {
            if (IsEmpty) return new byte[] { 0 };

            var res = new List<byte>();

            if (summary[1] != null)
            {
                var s = summary[1].ToBinary();
                res.AddRange(BitConverter.GetBytes((ushort)(s.Length + 3)));
                res.Add(0);
                res.AddRange(s);
            }
            if (summary[2] != null)
            {
                var s = summary[2].ToBinary();
                res.AddRange(BitConverter.GetBytes((ushort)(s.Length + 3)));
                res.Add(1);
                res.AddRange(s);
            }

            return res.ToArray();
        }
        public override string ToString()
        {
            if (IsEmpty) return "Empty";

            var str = "";
            if (summary[1] != null)
                str = $"First: {summary[1]}; ";
            if (summary[2] != null)
                str += $"Second: {summary[2]}";

            return str;
        }
    }

    [Serializable]
    public class Summary : IReadOnlyList<MatchEvent>
    {
        readonly IList<MatchEvent> eventList;

        public int Count => eventList.Count;
        public MatchEvent this[int index] => eventList[index];

        public Summary() => eventList = new List<MatchEvent>();
        Summary(IEnumerable<MatchEvent> matchEvents) => eventList = new List<MatchEvent>(matchEvents);

        public void Add(MatchEvent me)
        {
            eventList.Add(me);
        }
        public byte[] ToBinary()
        {
            if (eventList.Count == 0) return null;

            var res = new List<byte>(eventList.Count * 5);
            foreach (var me in eventList)
            {
                var nameBytes = Encoding.UTF8.GetBytes(me.PersonName);
                res.Add((byte)(nameBytes.Length + 4));
                res.Add(me.Time);
                res.Add(me.TeamID);
                res.Add((byte)me.Type);
                res.AddRange(nameBytes);
            }
            return res.ToArray();
        }
        public static Summary ToObject(byte[] bytes, int start, int length)
        {
            if (bytes is null) return null;
            var res = new Summary();

            try
            {
                for (int i = start; i < length;)
                {
                    res.Add(new MatchEvent(bytes[i + 1],
                                           bytes[i + 2],
                                           (MatchEventType)bytes[i + 3],
                                           Encoding.UTF8.GetString(bytes, i + 4, bytes[i] - 4)));
                    i += bytes[i];
                }
            }
            catch
            {
                return null;
            }

            return res;
        }
        public static Summary operator +(Summary ms1, Summary ms2)
        {
            if (ms1 is null || ms2 is null) return null;
            return new Summary(ms1.eventList.Concat(ms2.eventList));
        }

        public IEnumerator<MatchEvent> GetEnumerator() => eventList?.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => eventList?.GetEnumerator();
        public override string ToString() => eventList.ToString();
    }

    [Serializable]
    public class MatchEvent : ISerializable
    {
        readonly byte data;
        public byte Time { get; }
        public byte TeamID => (byte)((data & 1) + 1);
        public MatchEventType Type => (MatchEventType)(data >> 1);
        public string PersonName { get; }

        public MatchEvent(byte time, byte teamID, MatchEventType type, string personName)
        {
            Time = time;
            PersonName = personName;

            var tempTeam = (byte)((teamID >> 1 | 0b1111_1110) & 1);
            var tempType = (byte)((byte)type << 1);

            data = (byte)(tempTeam | tempType);
        }

        public override string ToString()
        {
            return $"Time: {Time} TimeID: {TeamID} Name: {PersonName} Type: {Type}";
        }

        #region Serializable

        MatchEvent(SerializationInfo info, StreamingContext context)
        {
            SerializationReader r = SerializationReader.GetReader(info);
            Time = r.ReadByte();
            data = r.ReadByte();
            PersonName = r.ReadString();
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter w = SerializationWriter.GetWriter();
            w.Write(Time);
            w.Write(data);
            w.Write(PersonName);
            w.AddToInfo(info);
        }

        #endregion
    }

    /// <summary> События происходящие во время игры </summary>
    [Serializable, TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MatchEventType : byte
    {
        /// <summary> Желтые карточки </summary>
        [Description("Желтые карточки")]
        YellowCards = 1,
        /// <summary> Красные карточки </summary>
        [Description("Красные карточки")]
        RedCards = 2,
        /// <summary> Гол </summary>
        [Description("Гол")]
        Goal = 3,
        /// <summary> Автогол </summary>
        [Description("Автогол")]
        AutoGoal = 4,
        /// <summary> Пенальти </summary>
        [Description("Пенальти")]
        Penalty = 5,
        /// <summary> Замена </summary>
        [Description("Замена")]
        Replacement = 6
    }
}
