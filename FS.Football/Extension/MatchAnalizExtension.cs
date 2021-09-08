using FS.Core;
using System.Runtime.CompilerServices;

namespace FS.Football.Extension
{
    public static class MatchAnalizExtension
    {
        public static MatchResult GetMatchResult(this Match match, Half half = Half.Match)
        {
            if (match.Score.IsEmpty) return MatchResult.None;

            var score = match.Score[half];
            if (score.HasValue)
            {
                if (score.Value.First == score.Value.Second) return MatchResult.Draw;
                return score.Value.First > score.Value.Second ? MatchResult.VictoryFirstTeam : MatchResult.VictorySeconTeam;
            }
            return MatchResult.None;
        }
        public static MatchResult GetMatchResult(this Match match, out Team team, Half half = Half.Match)
        {
            team = null;
            if (match.Score.IsEmpty) return MatchResult.None;

            var score = match.Score[half];
            if (score.HasValue)
            {
                if (score.Value.First == score.Value.Second) return MatchResult.Draw;
                if (score.Value.First > score.Value.Second)
                {
                    team = match.FirstTeam;
                    return MatchResult.VictoryFirstTeam;
                }
                else
                {
                    team = match.SecondTeam;
                    return MatchResult.VictorySeconTeam;
                }
            }
            return MatchResult.None;
        }


        /// <summary>
        /// Тотал.
        /// </summary>
        /// <param name="total"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Total(this Match match, byte total, LessMoreEqual lessMore, Half half = Half.Match)
        {
            if (match.Score.IsEmpty) return false;

            var score = match.Score[half];
            if (score.HasValue)
            {
                var sum = score.Value.First + score.Value.Second;
                switch (lessMore)
                {
                    case LessMoreEqual.More: return sum > total;
                    case LessMoreEqual.Less: return sum < total;
                    case LessMoreEqual.Equal: return sum == total;
                }
            }
            return false;
        }
        /// <summary>
        /// Тотал забитых мячей.
        /// </summary>
        /// <param name="team">Футбольная команда</param>
        /// <param name="total"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool TotalClogged(this Match match, Team team, byte total, LessMoreEqual lessMore, Half half = Half.Match)
        {
            if (match.Score.IsEmpty) return false;

            var score = match.Score[half];
            if (score.HasValue)
            {
                var sum = match.FirstTeam == team ? score.Value.First : score.Value.Second;
                switch (lessMore)
                {
                    case LessMoreEqual.More: return sum > total;
                    case LessMoreEqual.Less: return sum < total;
                    case LessMoreEqual.Equal: return sum == total;
                }
            }
            return false;
        }
        /// <summary>
        /// Тотал пропущенных мячей.
        /// </summary>
        /// <param name="team">Футбольная команда</param>
        /// <param name="total"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool TotalMissed(this Match match, Team team, byte total, LessMoreEqual lessMore, Half half = Half.Match)
        {
            if (match.Score.IsEmpty) return false;

            var score = match.Score[half];
            if (score.HasValue)
            {
                var sum = match.FirstTeam == team ? score.Value.Second : score.Value.First;
                switch (lessMore)
                {
                    case LessMoreEqual.More: return sum > total;
                    case LessMoreEqual.Less: return sum < total;
                    case LessMoreEqual.Equal: return sum == total;
                }
            }
            return false;
        }
        /// <summary>
        /// Победа.
        /// </summary>
        /// <param name="team">Футбольная команда</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Victory(this Match match, Team team, Half half = Half.Match)
        {
            Team _team;
            var res = match.GetMatchResult(out _team, half);
            if (res == MatchResult.None || res == MatchResult.Draw) return false;
            return _team == team;
        }
        /// <summary>
        /// Поражение.
        /// </summary>
        /// <param name="team">Футбольная команда</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Defeat(this Match match, Team team, Half half = Half.Match)
        {
            Team _team;
            var res = match.GetMatchResult(out _team, half);
            if (res == MatchResult.None || res == MatchResult.Draw) return false;
            return _team != team;
        }
        /// <summary>
        /// Ничья.
        /// </summary>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Draw(this Match match, Half half = Half.Match)
        {
            return match.GetMatchResult(half) == MatchResult.Draw;
        }
        /// <summary>
        /// Обе команды забъют.
        /// </summary>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool BothWillScore(this Match match, Half half = Half.Match)
        {
            if (match.Score.IsEmpty) return false;

            var score = match.Score[half];
            if (score.HasValue && score.Value.First > 0 && score.Value.Second > 0) return true;
            return false;
        }
        /// <summary>
        /// Гол в промежуток времени.
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Goal(this Match match, byte beginTime, byte endTime)
        {
            if (match.Summary.IsEmpty) return false;

            foreach (var e in match.Summary[Half.Match])
            {
                if (e.Type != MatchEventType.Goal) continue;
                if (beginTime <= e.Time && e.Time <= endTime) return true;
            }

            return false;
        }

        #region match.Statistics

        /// <summary>
        /// Статистика.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <param name="StatisticType">Тип статистики</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Statistic(this Match match, ushort value, StatisticType StatisticType, LessMoreEqual lessMore, Half half = Half.Match)
        {
            if (match.Statistic.IsEmpty) return false;
            var stat = match.Statistic[half];
            if (stat != null)
            {
                var statValue = stat[StatisticType];
                if (statValue.HasValue)
                {
                    var sum = statValue.Value.Sum;
                    switch (lessMore)
                    {
                        case LessMoreEqual.More: return sum > value;
                        case LessMoreEqual.Less: return sum < value;
                        case LessMoreEqual.Equal: return sum == value;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Статистика. Владение мячом.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool BallPossession(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.BallPossession, lessMore, half);
        }
        /// <summary>
        /// Статистика. Удары.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool GoalAttempts(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.GoalAttempts, lessMore, half);
        }
        /// <summary>
        /// Статистика. Удары в створ.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool ShotsOnGoal(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.ShotsOnGoal, lessMore, half);
        }
        /// <summary>
        /// Статистика. Удары мимо.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool ShotsOffGoal(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.ShotsOffGoal, lessMore, half);
        }
        /// <summary>
        /// Статистика. Блоковано ударов.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool BlockedStrokes(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.BlockedStrokes, lessMore, half);
        }
        /// <summary>
        /// Статистика. Штрафные.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool FreeKicks(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.FreeKicks, lessMore, half);
        }
        /// <summary>
        /// Статистика. Угровые.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool CornerKicks(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.CornerKicks, lessMore, half);
        }
        /// <summary>
        /// Статистика. Офсайды.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Offside(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.Offside, lessMore, half);
        }
        /// <summary>
        /// Статистика. Вбрасывания.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool ThrowIn(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.ThrowIn, lessMore, half);
        }
        /// <summary>
        /// Статистика. Сэйвы.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Saves(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.Saves, lessMore, half);
        }
        /// <summary>
        /// Статистика. Фолы.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Fouls(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.Fouls, lessMore, half);
        }
        /// <summary>
        /// Статистика. Желтые карточки.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool YellowCards(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.YellowCards, lessMore, half);
        }
        /// <summary>
        /// Статистика. Красные карточки.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool RedCards(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.RedCards, lessMore, half);
        }
        /// <summary>
        /// Статистика. Всего передач.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool TotalPasses(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.TotalPasses, lessMore, half);
        }
        /// <summary>
        /// Статистика. Отборы.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Selections(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.Selections, lessMore, half);
        }
        /// <summary>
        /// Статистика. Атаки.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool Attacks(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.Attacks, lessMore, half);
        }
        /// <summary>
        /// Статистика. Опасные атаки.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lessMore">Меньше, Больше или Равно</param>
        /// <param name="half">Тайм</param>
        /// <returns>True, если матч удовлетворяет условию.</returns>
        public static bool DangerousAttacks(this Match match, ushort value, LessMoreEqual lessMore, Half half = Half.Match)
        {
            return match.Statistic(value, StatisticType.DangerousAttacks, lessMore, half);
        }

        #endregion
    }
}
