using FlashScore.Serialization;
using FlashScore.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FlashScore.Football
{
    [Serializable]
    public sealed class Match : ISerializable
    {
        private ScoreMatch score;
        private MatchStatistic statistic;
        private MatchSummary summary;

        public Match(FSID id, string country, string championship, string firstTeam, string secondTeam, int time)
            : this(id, country, championship, new Team(firstTeam), new Team(secondTeam), new DateTime(1970, 1, 1).AddSeconds(time)) { }
        public Match(FSID id, string country, string championship, string firstTeam, string secondTeam, DateTime time)
           : this(id, country, championship, new Team(firstTeam), new Team(secondTeam), time) { }
        public Match(FSID id, string country, string championship, Team firstTeam, Team secondTeam, int time)
            : this(id, country, championship, firstTeam, secondTeam, new DateTime(1970, 1, 1).AddSeconds(time)) { }
        public Match(FSID id, string country, string championship, Team firstTeam, Team secondTeam, DateTime time)
        {
            Id = id;
            Country = Helper.HasNullOrEmpty(country);
            Championship = Helper.HasNullOrEmpty(championship);
            FirstTeam = firstTeam.HasNull(nameof(firstTeam));
            SecondTeam = secondTeam.HasNull(nameof(secondTeam));
            Time = time;
        }

        internal bool Checked { get; set; }
        public FSID Id { get; private set; }
        public string Country { get; internal set; }
        public string Championship { get; internal set; }
        public Team FirstTeam { get; internal set; }
        public Team SecondTeam { get; internal set; }
        public DateTime Time { get; private set; }
        public ScoreMatch Score { get => score; set => score = value.HasNull(nameof(Score)); }
        public MatchStatistic Statistic { get => statistic; set => statistic = value.HasNull(nameof(Statistic)); }
        public MatchSummary Summary
        {
            get => summary;
            set
            {
                summary = value.HasNull(nameof(Summary));
                if (!summary.IsEmpty)
                    Score.UpdateScore(summary);
            }
        }
        public IList<Match> LastPersonalMatches { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(FirstTeam);

            if(Score is null) sb.Append(" - ");
            else sb.AppendFormat(" {0} ",Score[Half.Match].Value);

            sb.Append(SecondTeam);

            return sb.ToString();
        }

        #region Serializable

        Match(SerializationInfo info, StreamingContext context)
        {
            SerializationReader r = SerializationReader.GetReader(info);
            Id = new(r.ReadInt64());
            Country = r.ReadString();
            Championship = r.ReadString();
            Checked = r.ReadBoolean();
            Time = r.ReadDateTime();
            LastPersonalMatches = r.ReadList<Match>() as MatchCollection;
            FirstTeam = r.ReadObject() as Team;
            SecondTeam = r.ReadObject() as Team;
            score = r.ReadObject() as ScoreMatch;
            statistic = r.ReadObject() as MatchStatistic;
            summary = r.ReadObject() as MatchSummary;
            if (summary != null) score.UpdateScore(summary);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var w = SerializationWriter.GetWriter();
            w.Write(Id.ToInt64());
            w.Write(Country);
            w.Write(Championship);
            w.Write(Checked);
            w.Write(Time);
            w.Write(LastPersonalMatches);
            w.WriteObject(FirstTeam);
            w.WriteObject(SecondTeam);
            w.WriteObject(Score);
            w.WriteObject(Statistic);
            w.WriteObject(Summary);
            w.AddToInfo(info);
        }

        #endregion
    }
}
