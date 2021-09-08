using MSFootball.Models.Data;
using System;
using System.Collections.Generic;
using Regular = System.Text.RegularExpressions;

namespace MSFootball.Models.Parsing
{
    class RegularMatchesParser : IDataParser<List<CurrentMatch>>
    {
        public List<CurrentMatch> Parse(string source)
        {
            string name1, name2, url, championship, country, country_championship;
            int time;
            string[] gs;
            List<CurrentMatch> matches = new List<CurrentMatch>();

            // разбиваем данные на масив матчей по чемпионатам
            string[] allMatch = source.Split(new string[] { "~ZA÷" }, StringSplitOptions.RemoveEmptyEntries);


            for (int i = 1; i < allMatch.Length; i++)
            {
                country_championship = allMatch[i].Split(new string[] { "¬ZEE÷" }, StringSplitOptions.RemoveEmptyEntries)[0];
                // Получаем страну и чемпионат
                country = country_championship.Split(':')[0].ToUpper();
                championship = country_championship.Split(':')[1].Remove(0, 1).Split('¬')[0];

                // получаем массив игр в чемпионате
                gs = allMatch[i].Split(new string[] { "~AA÷" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 1; j < gs.Length; j++)
                {
                    time = int.Parse(Regular.Regex.Match(gs[j], @"([0-9]{10})").Groups[1].Value);
                    name1 = Regular.Regex.Match(gs[j], @"AE÷(.*)¬JA").Groups[1].Value;
                    name2 = Regular.Regex.Match(gs[j], @"AF÷(.*)¬JB").Groups[1].Value;
                    url = gs[j].Split('¬')[0];

                    //score = string.Format($"{Regular.Regex.Match(gs[j], @"AG÷([0-9]{1,2})¬").Groups[1].Value}-{Regular.Regex.Match(gs[j], @"AH÷([0-9]{1,2})¬").Groups[1].Value}");

                    // Если игра уже закончилась пропускаем ее
                    //if (score != "-" && !gs[i].Contains("AO")) continue;

                    // Добавляем в список
                    var match = new CurrentMatch(new MatchId(url), country, championship, MatchBase.ParseDT(time), new Team(name1), new Team(name2));
                    matches.Add(match);
                }
            }
            return matches;
        }
    }
}
