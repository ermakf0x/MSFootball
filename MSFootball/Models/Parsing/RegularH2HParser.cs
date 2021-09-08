using MSFootball.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSFootball.Models.Parsing
{
    class RegularH2HParser : IDataParser<H2HParserResult>
    {
        public H2HParserResult Parse(string source)
        {
            string[] lastMatchesTeams = source.Split(new string[] { "KA÷" }, StringSplitOptions.RemoveEmptyEntries)[0]
                                              .Split(new string[] { "~KB÷" }, StringSplitOptions.RemoveEmptyEntries);

            var first = GetMatchesTeam(lastMatchesTeams[1]);
            var second = GetMatchesTeam(lastMatchesTeams[2]);
            List<EndedMatch> personal = null;
            if (lastMatchesTeams.Length > 3)
                personal = GetMatchesTeam(lastMatchesTeams[3]);

            return new H2HParserResult(first, second, personal);
        }
        List<EndedMatch> GetMatchesTeam(string source)
        {
            string name1, name2, url, championship, country, scoreStr;
            string[] temp;
            int time;
            var matches = new List<EndedMatch>();

            string[] lastMatches = source.Split(new string[] { "~KC÷" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < lastMatches.Length; i++)
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

                var score = ScoreMatch.ParseScore(scoreStr);
                ScoreMatch scoreMatch = null;
                if (score.HasValue)
                    scoreMatch = new ScoreMatch(score.Value);

                matches.Add(new EndedMatch(new MatchId(url), country, championship, MatchBase.ParseDT(time), name1, name2, scoreMatch));
            }

            return matches;
        }
    }
}
