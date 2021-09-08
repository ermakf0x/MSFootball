using System;
using System.Collections.Generic;

namespace ConsoleTesting
{
    public sealed class Match
    {
        static readonly HashSet<Match> matchSet = new();
        static readonly HashSet<string> countrySet = new();
        static readonly HashSet<string> championshipSet = new();
        public static IReadOnlyCollection<Match> All => matchSet;

        public FSID Id { get; }
        public string Country { get; }
        public string Championship { get; }
        public Team FirstTeam { get; }
        public Team SecondTeam { get; }
        public DateTime Time { get; }
        public List<Match> LastPersonalMatches { get; set; }
        public ScoreMatch Score { get; set; }
        public MatchSummary Summary { get; set; }

        Match(FSID id, string country, string championship, Team firstTeam, Team secondTeam, DateTime time)
        {
            Id = id;
            Country = country;
            Championship = championship;
            FirstTeam = firstTeam;
            SecondTeam = secondTeam;
            Time = time;
        }

        public static Match New(FSID id, string country, string championship, Team firstTeam, Team secondTeam, DateTime time)
        {
            string _country = country, _championship = championship;

            if (countrySet.TryGetValue(country, out string c)) _country = c;
            else countrySet.Add(_country);
            
            if (countrySet.TryGetValue(championship, out string c2)) _championship = c2;
            else championshipSet.Add(_championship);

            var match = new Match(id, _country, _championship, firstTeam, secondTeam, time);
            if (matchSet.TryGetValue(match, out Match output)) return output;
            matchSet.Add(match);
            return match;
        }

        public override bool Equals(object obj)
        {
            if (obj is Match match && match != null) return Id.Equals(match.Id);
            return false;
        }
        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString()
        {
            return $"Url: {Id}, Country: {Country}, Championship: {Championship}, T1: {FirstTeam.Name}, T2: {SecondTeam.Name}";
        }
    }
}