using FlashScore.Utils;
using System;
using System.Collections.Generic;

namespace FlashScore.Football
{
    /// <summary> Уникальный набор футбольных матчей. </summary>
    public static class FootballMatchSet
    {
        static readonly Dictionary<FSID, Match> _matches   = new();
        static readonly Dictionary<string, Team> _teams    = new();
        static readonly HashSet<string> _countries         = new();
        static readonly HashSet<string> _championships     = new();

        public static Match NewMatch(FSID id, string country, string championship, Team firstTeam, Team secondTeam, int time)
        {
            if (_matches.ContainsKey(id)) return _matches[id];

            Helper.ThrowIfNullOrEmpty(country);
            Helper.ThrowIfNullOrEmpty(championship);
            Helper.ThrowIfNull(firstTeam, nameof(firstTeam));
            Helper.ThrowIfNull(secondTeam, nameof(secondTeam));

            var actualCountry = _countries.TryGetValue(country, out string c) ? c : country;
            var actualChampionship = _championships.TryGetValue(championship, out string c2) ? c2 : championship;

            var newMatch = new Match(id, actualCountry, actualChampionship, firstTeam, secondTeam, time);
            _matches.Add(id, newMatch);
            return newMatch;
        }
        public static Match NewMatch(FSID id, string country, string championship, Team firstTeam, Team secondTeam, DateTime time)
        {
            if (_matches.ContainsKey(id)) return _matches[id];

            Helper.ThrowIfNullOrEmpty(country);
            Helper.ThrowIfNullOrEmpty(championship);
            Helper.ThrowIfNull(firstTeam, nameof(firstTeam));
            Helper.ThrowIfNull(secondTeam, nameof(secondTeam));

            var actualCountry = _countries.TryGetValue(country, out string c) ? c : country;
            var actualChampionship = _championships.TryGetValue(championship, out string c2) ? c2 : championship;

            var newMatch = new Match(id, actualCountry, actualChampionship, firstTeam, secondTeam, time);
            _matches.Add(id, newMatch);
            return newMatch;
        }

        public static Team NewTeam(string name)
        {
            Helper.ThrowIfNullOrEmpty(name);
            if (_teams.ContainsKey(name)) return _teams[name];
            var newTeam = new Team(name);
            _teams.Add(name, newTeam);
            return newTeam;
        }
    }
}
