using System.Collections;
using System.Collections.Generic;

namespace FS.Football
{
    public sealed class FootballMatchTable
    {
        readonly HashSet<Match> matches;
        readonly HashSet<Team> teams;
        readonly HashSet<string> countries;
        readonly HashSet<string> championships;

        public int Count => matches.Count;

        public FootballMatchTable()
        {
            matches = new();
            teams = new();
            countries = new();
            championships = new();
        }

        public Match Push(Match match)
        {
            return Add(match, true);
        }
        Match Add(Match match, bool recursive)
        {
            if (matches.TryGetValue(match, out Match res)) return res;

            if (countries.TryGetValue(match.Country, out string country)) match.Country = country;
            else countries.Add(match.Country);

            if (championships.TryGetValue(match.Championship, out string championship)) match.Championship = championship;
            else championships.Add(match.Championship);

            if (teams.TryGetValue(match.FirstTeam, out Team t1)) match.FirstTeam = t1;
            else teams.Add(match.FirstTeam);

            if (teams.TryGetValue(match.SecondTeam, out Team t2)) match.SecondTeam = t2;
            else teams.Add(match.SecondTeam);

            matches.Add(match);

            if (match.LastPersonalMatches != null) match.LastPersonalMatches = AddRange(match.LastPersonalMatches);
            if (match.FirstTeam.LastMatches != null) match.FirstTeam.LastMatches = AddRange(match.FirstTeam.LastMatches);
            if (match.SecondTeam.LastMatches != null) match.SecondTeam.LastMatches = AddRange(match.SecondTeam.LastMatches);

            return match;
        }

        public IList<Match> AddRange(IList<Match> matches)
        {
            return AddRange(matches, true);
        }
        IList<Match> AddRange(IList<Match> matches, bool recursive)
        {
            if (matches is null) return null;

            var res = new MatchCollection(matches.Count);
            foreach (var match in matches)
                res.Add(Add(match, recursive));
            return res;
        }
    }

    public abstract class HashCollection<TData, TBuilder> where TBuilder : HashCollection<TData, TBuilder>.Bulder
    {
        protected readonly HashSet<TData> data;

        public HashCollection()
        {
            data = new HashSet<TData>();
        }

        public abstract TBuilder GetBuilder();

        public abstract class Bulder
        {
            public abstract TData Build();
        }
    }

    class MyCollection : HashCollection<Match, MyCollection.MyBuilder>
    {
        public override MyBuilder GetBuilder()
        {
            return new MyBuilder();
        }

        public class MyBuilder : Bulder
        {
            public override Match Build()
            {
                return null;
            }
        }
    }
}
