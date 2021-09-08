using System;

namespace MSFootball.Models.Data
{
    [Serializable]
    /// <summary>
    /// Футбольный матч, который уже сыгран.
    /// </summary>
    public class EndedMatch : MatchBase
    {
        private MatchSummary summary;

        /// <summary>
        /// Имя первой команды
        /// </summary>
        public string TeamName1 { get; set; }
        /// <summary>
        /// Имя второй команды
        /// </summary>
        public string TeamName2 { get; set; }
        /// <summary>
        /// Игровой счет за матч
        /// </summary>
        public ScoreMatch Score { get; set; }
        /// <summary>
        /// Статистика за матч
        /// </summary>
        public MatchStatistic Statistic { get; set; }
        /// <summary>
        /// Обзор матча
        /// </summary>
        public MatchSummary Summary
        {
            get => summary;
            set
            {
                summary = value;
                if (summary is null || Score is null) return;

                if(summary[Half.First] != null)
                {
                    byte s1 = 0, s2 = 0;
                    foreach (var a in summary[Half.First].EventList)
                    {
                        if(a.Type == MatchEventType.Goal || a.Type == MatchEventType.AutoGoal)
                        {
                            if (a.TeamID == 1) s1++;
                            else s2++;
                        }
                    }
                    Score.AddFirst(new Score(s1, s2));
                }

                if (summary[Half.Second] != null)
                {
                    byte s1 = 0, s2 = 0;
                    foreach (var a in summary[Half.Second].EventList)
                    {
                        if (a.Type == MatchEventType.Goal || a.Type == MatchEventType.AutoGoal)
                        {
                            if (a.TeamID == 1) s1++;
                            else s2++;
                        }
                    }
                    Score.AddSecond(new Score(s1, s2));
                }
            }
        }

        public EndedMatch(MatchId id, string country, string championship, DateTime time, string teamName1, string teamName2, ScoreMatch score)
                   : base(id, country, championship, time)
        {
            TeamName1 = teamName1 ?? throw new ArgumentNullException(nameof(teamName1));
            TeamName2 = teamName2 ?? throw new ArgumentNullException(nameof(teamName2));
            Score = score;// ?? throw new ArgumentNullException(nameof(score));
        }

        /// <summary>
        /// Возвращает исход матча
        /// </summary>
        /// <param name="half"></param>
        /// <returns>Имя победившей команды или null - нет данных, string.Empty - ничья.</returns>
        public string GetResult(Half half)
        {
            if (Score is null) return null;

            var score = Score[half];
            if (score is null) return null;

            if (score.Value.First > score.Value.Second) return TeamName1;
            else if (score.Value.First < score.Value.Second) return TeamName2;
            else return string.Empty;
        }

        public override string ToString()
        {
            return $"{TeamName1} - {TeamName2}";
        }
    }
}
