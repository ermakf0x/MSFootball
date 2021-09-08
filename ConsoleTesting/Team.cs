using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleTesting
{
    public record Team
    {
        static readonly HashSet<Team> teamSet = new(new Comparer());
        public static IReadOnlyCollection<Team> All => teamSet;

        public string Name { get; }
        public List<Match> LastMatches { get; set; }

        Team(string name) => Name = name;

        public static Team New(string name)
        {
            var team = new Team(name);
            if (teamSet.TryGetValue(team, out Team output)) return output;
            teamSet.Add(team);
            return team;
        }

        class Comparer : IEqualityComparer<Team>
        {
            public bool Equals(Team x, Team y) => x.Name == y.Name;
            public int GetHashCode([DisallowNull] Team obj) => obj.Name.GetHashCode();
        }
    }
}