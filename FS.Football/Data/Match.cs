using FS.Core.Serialization;
using FS.Football.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FS.Football
{
    [Serializable]
    public sealed class Match : ISerializable
    {
        private ScoreMatch score;
        private MatchStatistic statistic;
        private MatchSummary summary;

        public Match(MatchUrl url, string country, string championship, string firstTeam, string secondTeam, int time)
            : this(url, country, championship, new Team(firstTeam), new Team(secondTeam), new DateTime(1970, 1, 1).AddSeconds(time)) { }
        public Match(MatchUrl url, string country, string championship, string firstTeam, string secondTeam, DateTime time)
           : this(url, country, championship, new Team(firstTeam), new Team(secondTeam), time) { }
        public Match(MatchUrl url, string country, string championship, Team firstTeam, Team secondTeam, int time)
            : this(url, country, championship, firstTeam, secondTeam, new DateTime(1970, 1, 1).AddSeconds(time)) { }
        public Match(MatchUrl url, string country, string championship, Team firstTeam, Team secondTeam, DateTime time)
        {
            Url = url.HasNull(nameof(url));
            Country = Helper.HasNullOrWhiteSpace(country);
            Championship = Helper.HasNullOrWhiteSpace(championship);
            FirstTeam = firstTeam.HasNull(nameof(firstTeam));
            SecondTeam = secondTeam.HasNull(nameof(secondTeam));
            Time = time;
        }

        internal bool Checked { get; set; }
        public MatchUrl Url { get; private set; }
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
            Country = r.ReadString();
            Championship = r.ReadString();
            Checked = r.ReadBoolean();
            Time = r.ReadDateTime();
            LastPersonalMatches = r.ReadList<Match>() as MatchCollection;
            Url = r.ReadObject() as MatchUrl;
            FirstTeam = r.ReadObject() as Team;
            SecondTeam = r.ReadObject() as Team;
            score = r.ReadObject() as ScoreMatch;
            statistic = r.ReadObject() as MatchStatistic;
            summary = r.ReadObject() as MatchSummary;
            if (summary != null) score.UpdateScore(summary);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter w = SerializationWriter.GetWriter();
            w.Write(Country);
            w.Write(Championship);
            w.Write(Checked);
            w.Write(Time);
            w.Write(LastPersonalMatches);
            w.WriteObject(Url);
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
