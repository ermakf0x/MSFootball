using MSFootball.Converters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MSFootball.Models.Data
{
    /// <summary>
    /// Обзор игровых событий
    /// </summary>
    [Serializable]
    public class MatchSummary
    {
        readonly Summary[] summary;
        public bool IsEmpty { get; private set; }
        public static MatchSummary Empty = new MatchSummary(null, null) { IsEmpty = true };

        private MatchSummary(Summary first, Summary second)
        {
            summary = new Summary[3];
            summary[1] = first;
            summary[2] = second;
        }
        public static MatchSummary Build(Summary first, Summary second)
        {
            if (first is null && second is null) return new MatchSummary(null, null) { IsEmpty = true };
            return new MatchSummary(first, second);
        }

        public Summary this[Half half]
        {
            get
            {
                if (IsEmpty) return null;
                switch (half)
                {
                    case Half.Match: return GetSummaryAllMatch();
                    case Half.First: return summary[1];
                    case Half.Second: return summary[2];
                }
                return null;
            }
        }

        Summary GetSummaryAllMatch()
        {
            if (summary[0] != null) return summary[0];

            if (summary[1] == null || summary[2] == null)
            {
                if(summary[1] == null)
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
            if (bytes is null || bytes.Length == 0) return null;
            if (bytes.Length == 1 && bytes[0] == 0)
                return new MatchSummary(null, null) { IsEmpty = true };

            var res = new Summary[2];

            var indent = BitConverter.ToUInt16(bytes, 0);
            var s = Summary.ToObject(bytes, 3, indent);
            if(s != null && bytes[2] <= 2)
            {
                res[bytes[2] + 1] = s;
            }

            if (indent < bytes.Length)
            {
                s = Summary.ToObject(bytes, indent + 3, bytes.Length);
                if (s != null && bytes[indent + 2] <= 2)
                {
                    res[bytes[indent + 2] + 1] = s;
                }
            }

            return Build(res[0], res[1]);
        }
        public byte[] ToBinary()
        {
            if (IsEmpty)
                return new byte[] { 0 };

            var res = new List<byte>();

            if (summary[1] != null)
            {
                var s = summary[0].ToBinary();
                res.AddRange(BitConverter.GetBytes((ushort)(s.Length + 3)));
                res.Add(0);
                res.AddRange(s);
            }
            if (summary[2] != null)
            {
                var s = summary[1].ToBinary();
                res.AddRange(BitConverter.GetBytes((ushort)(s.Length + 3)));
                res.Add(1);
                res.AddRange(s);
            }

            return res.ToArray();
        }
        public override string ToString()
        {
            if (IsEmpty) return "Empty";

            var str = string.Empty;
            if (summary[0] != null)
                str += $"First: {summary[0]}; ";
            if (summary[0] != null)
                str += $"Second: {summary[1]}";

            return str;
        }


        
    }

    [Serializable]
    public class Summary
    {
        List<MatchEvent> _eventList;
        public IReadOnlyList<MatchEvent> EventList { get; private set; }

        public Summary()
        {
            _eventList = new List<MatchEvent>();
            EventList = _eventList;
        }
        Summary(IEnumerable<MatchEvent> matchEvents)
        {
            _eventList = new List<MatchEvent>(matchEvents);
            EventList = _eventList;
        }

        public void Add(MatchEvent me)
        {
            _eventList.Add(me);
        }
        public byte[] ToBinary()
        {
            if (_eventList is null || _eventList.Count == 0) return null;

            var res = new List<byte>(_eventList.Count * 5);
            foreach (var e in _eventList)
            {
                var nameBytes = Encoding.UTF8.GetBytes(e.PersonName);
                res.Add((byte)(nameBytes.Length + 4));
                res.Add(e.Time);
                res.Add(e.TeamID);
                res.Add((byte)e.Type);
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
            return new Summary(ms1._eventList.Concat(ms2._eventList));
        }
        public override string ToString()
        {
            return _eventList.Count.ToString();
        }
    }

    [Serializable]
    public class MatchEvent
    {
        public byte Time { get; set; }
        public byte TeamID { get; set; }
        public MatchEventType Type { get; set; }
        public string PersonName { get; set; }

        public MatchEvent(byte time, byte teamID, MatchEventType type, string personName)
        {
            Time = time;
            TeamID = teamID;
            Type = type;
            PersonName = personName;
        }

        public override string ToString()
        {
            return $"Time: {Time} TimeID: {TeamID} Name: {PersonName} Type: {Type}";
        }
    }

    /// <summary>
    /// События происходящие во время игры
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MatchEventType
    {
        /// <summary>
        /// Желтые карточки
        /// </summary>
        [Description("Желтые карточки")]
        YellowCards = 1,
        /// <summary>
        /// Красные карточки
        /// </summary>
        [Description("Красные карточки")]
        RedCards = 2,
        /// <summary>
        /// Гол
        /// </summary>
        [Description("Гол")]
        Goal = 3,
        /// <summary>
        /// Автогол
        /// </summary>
        [Description("Автогол")]
        AutoGoal = 4,
        /// <summary>
        /// Пенальти
        /// </summary>
        [Description("Пенальти")]
        Penalty = 5,
        /// <summary>
        /// Замена
        /// </summary>
        [Description("Замена")]
        Replacement = 6
    }
}
