using System;
using Redex = System.Text.RegularExpressions;

namespace FlashScore.Football.Parsing
{
    public class RegularStatisticParser : IDataParser<MatchStatistic>
    {
        public MatchStatistic Parse(string source)
        {
            if (string.IsNullOrEmpty(source)) return MatchStatistic.Empty;

            Statistic match = null;
            Statistic first = null;
            Statistic second = null;

            try
            {
                var stats = source.Split(new string[] { "~SE÷" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in stats)
                {
                    if (s.Contains("Матч")) match = GetStatistic(s);
                    else if (s.Contains("1-й тайм")) first = GetStatistic(s);
                    else if (s.Contains("2-й тайм")) second = GetStatistic(s);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(source + "\n");
            }

            return new MatchStatistic(match, first, second);
        }
        Statistic GetStatistic(string source)
        {
            var res = new Statistic();
            string name; string[] temp; Redex.MatchCollection mc;

            var stats = source.Split(new string[] { "~SG÷" }, StringSplitOptions.RemoveEmptyEntries);
            
            for (int i = 1; i < stats.Length; i++)
            {
                temp = stats[i].Split('¬');
                name = temp[0];
                mc = Redex.Regex.Matches(stats[i], @"([0-9]{1,3})");
                var value = new StatisticValue(ushort.Parse(mc[0].Value), ushort.Parse(mc[1].Value));
                switch (name)
                {
                    case "Владение мячом": res[StatisticType.BallPossession]    = value; break;
                    case "Удары": res[StatisticType.GoalAttempts]               = value; break;
                    case "Удары в створ": res[StatisticType.ShotsOnGoal]        = value; break;
                    case "Удары мимо": res[StatisticType.ShotsOffGoal]          = value; break;
                    case "Блок-но ударов": res[StatisticType.BlockedStrokes]    = value; break;
                    case "Штрафные": res[StatisticType.FreeKicks]               = value; break;
                    case "Угловые": res[StatisticType.CornerKicks]              = value; break;
                    case "Офсайды": res[StatisticType.Offside]                  = value; break;
                    case "Вбрасывания": res[StatisticType.ThrowIn]              = value; break;
                    case "Сэйвы": res[StatisticType.Saves]                      = value; break;
                    case "Фолы": res[StatisticType.Fouls]                       = value; break;
                    case "Желтые карточки": res[StatisticType.YellowCards]      = value; break;
                    case "Красные карточки": res[StatisticType.RedCards]        = value; break;
                    case "Всего передач": res[StatisticType.TotalPasses]        = value; break;
                    case "Oтборы": res[StatisticType.Selections]                = value; break;
                    case "Атаки": res[StatisticType.Attacks]                    = value; break;
                    case "Опасные атаки": res[StatisticType.DangerousAttacks]   = value; break;
                    default:
                        //res[name] = new StatisticValue(ushort.Parse(mc[0].Value), ushort.Parse(mc[1].Value));
                        break;
                }
            }

            return res;
        }
    }
}
