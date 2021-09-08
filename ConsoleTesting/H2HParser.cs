using System;
using System.Collections.Generic;

namespace ConsoleTesting
{
    class H2HParser : FS.Football.Parsing.IDataParser<H2HParser.Result>
    {
        public Result Parse(string source)
        {
            string[] lastMatchesTeams = source.Split(new string[] { "KA÷" }, StringSplitOptions.RemoveEmptyEntries)[0]
                                              .Split(new string[] { "~KB÷" }, StringSplitOptions.RemoveEmptyEntries);

            var first = GetMatchesTeam(lastMatchesTeams[1]);
            var second = GetMatchesTeam(lastMatchesTeams[2]);
            List<Match> personal = null;
            if (lastMatchesTeams.Length > 3)
                personal = GetMatchesTeam(lastMatchesTeams[3]);

            return new(personal, first, second);
        }
        static List<Match> GetMatchesTeam(string source)
        {
            string name1, name2, url, championship, country, scoreStr;
            string[] temp;
            int time;
            var matches = new List<Match>();

            string[] lastMatches = source.Split(new string[] { "~KC÷" }, StringSplitOptions.RemoveEmptyEntries);
            var length = Math.Min(21, lastMatches.Length);


            for (int i = 1; i < length; i++)
            {
                temp = lastMatches[i].Split('¬');

                url = temp[1][3..];
                time = int.Parse(temp[0]);
                championship = temp[2][3..];
                country = temp[4][3..];
                name1 = temp[6][3..];
                name2 = temp[7][3..];
                scoreStr = temp[8][3..];

                if (name1.Contains('*'))
                    name1 = name1[1..];
                if (name2.Contains('*'))
                    name2 = name2[1..];

                var match = Match.New(new(url), country, championship, Team.New(name1), Team.New(name2), new DateTime(1970, 1, 1).AddSeconds(time));
                match.Score = ParceScore(scoreStr);
                matches.Add(match);
            }

            return matches;
        }
        static ScoreMatch ParceScore(string scoreStr)
        {
            if (scoreStr.Contains("-")) return null;

            if (scoreStr.Contains("(")) // "3 : 2(2 : 2)"
            {
                var pS = ParseValue(scoreStr.Split('(')[0]);
                return new(new(pS[0], pS[1]));
            }
            else // "2 : 1"
            {
                var pS = ParseValue(scoreStr);
                return new(new(pS[0], pS[1]));
            }
        }
        static byte[] ParseValue(string score)
        {
            var res = new byte[2];
            var a = score.Split(':');

            res[0] = byte.Parse(a[0]);
            res[1] = byte.Parse(a[1]);

            return res;
        }

        public class Result
        {
            public List<Match> Personal { get; init; }
            public List<Match> First { get; init; }
            public List<Match> Second { get; init; }

            public Result(List<Match> personal, List<Match> first, List<Match> second)
            {
                Personal = personal;
                First = first;
                Second = second;
            }

            public override string ToString() => $"Personal: {Personal?.Count}, First: {First?.Count}, Second: {Second?.Count}";
        }
    }
}