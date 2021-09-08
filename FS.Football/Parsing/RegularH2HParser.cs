using System;
using System.Collections.Generic;

namespace FS.Football.Parsing
{
    public class RegularH2HParser : IDataParser<H2HParserResult>
    {
        public H2HParserResult Parse(string source)
        {
            string[] lastMatchesTeams = source.Split(new string[] { "KA÷" }, StringSplitOptions.RemoveEmptyEntries)[0]
                                              .Split(new string[] { "~KB÷" }, StringSplitOptions.RemoveEmptyEntries);

            var first = GetMatchesTeam(lastMatchesTeams[1]);
            var second = GetMatchesTeam(lastMatchesTeams[2]);
            IList<Match> personal = null;
            if (lastMatchesTeams.Length > 3)
                personal = GetMatchesTeam(lastMatchesTeams[3]);

            return new H2HParserResult(first, second, personal);
        }

        static MatchCollection GetMatchesTeam(string source)
        {
            string name1, name2, url, championship, country, scoreStr;
            string[] temp;
            int time;
            var matches = new MatchCollection();

            string[] lastMatches = source.Split(new string[] { "~KC÷" }, StringSplitOptions.RemoveEmptyEntries);
            var length = Math.Min(20, lastMatches.Length);


            for (int i = 1; i < length; i++)
            {
                temp = lastMatches[i].Split('¬');

                url = temp[1].Substring(3);
                time = int.Parse(temp[0]);
                championship = temp[2].Substring(3);
                country = temp[4].Substring(3);
                name1 = temp[6].Substring(3);
                name2 = temp[7].Substring(3);
                scoreStr = temp[8].Substring(3);

                if (name1.Contains('*'))
                    name1 = name1.Substring(1);
                if (name2.Contains('*'))
                    name2 = name2.Substring(1);

                var score = ScoreMatch.Parse(scoreStr);

                matches.Add(new Match(new MatchUrl(url), country, championship, name1, name2, time) { Score = score });
            }

            return matches;
        }
    }
}
