using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace ConsoleTesting
{
    [Serializable]
    public class MatchSummary : IReadOnlyCollection<MatchSummary.Event>
    {
        public IReadOnlyCollection<Event> First { get; }
        public IReadOnlyCollection<Event> Second { get; }
        public int Count => (First?.Count + Second?.Count) ?? 0;

        public MatchSummary(IReadOnlyCollection<Event> first, IReadOnlyCollection<Event> second) => (First, Second) = (first, second);

        public override string ToString() => $"match: {Count}, first: {First?.Count ?? 0}, second: {Second?.Count ?? 0}";
        public IEnumerator<Event> GetEnumerator()
        {
            if (First != null) foreach (var me in First) yield return me;
            if (Second != null) foreach (var me in Second) yield return me;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        [Serializable]
        public readonly struct Event
        {
            readonly byte data;
            public readonly byte time;
            public readonly string personName;
            public byte TeamID => (byte)((data & 1) + 1);
            public EventType Type => (EventType)(data >> 1);

            public Event(byte time, byte teamID, EventType type, string personName)
            {
                this.time = time;
                this.personName = personName;

                var tempTeam = (byte)((teamID >> 1 | 0b1111_1110) & 1);
                var tempType = (byte)((byte)type << 1);

                data = (byte)(tempTeam | tempType);
            }
            public override string ToString() => $"Time: {time} TimeID: {TeamID} Name: {personName} Type: {Type}";
        }

        [Serializable]
        public enum EventType : byte
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
}
