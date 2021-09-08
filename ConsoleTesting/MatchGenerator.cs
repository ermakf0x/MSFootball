using System;
using System.Collections.Generic;

namespace ConsoleTesting
{
    class MatchGenerator
    {
        static readonly string[] countries = { "АВСТРАЛИЯ", "АНГЛИЯ", "АРГЕНТИНА", "БОЛГАРИЯ", "БРАЗИЛИЯ", "ГЕРМАНИЯ", "ИСПАНИЯ", "КОЛУМБИЯ",
        "ПЕРУ", "ПОЛЬША","ПОРТУГАЛИЯ","РУМЫНИЯ" };
        static readonly string[] teamnames = { "Мельбурн Сити", "Централ Кост Маринерс", "Бока Хуниорс", "Тальерес Кордоба", "Сармьенто Жунин",
        "Боливар", "Реал Потоси", "Индепендьенте", "Фортуна Дюссельдорф", "Бохум", "Сарагоса", "Мирандес", "Сантос Лагуна", "Леон" };

        static readonly Random rnd = new();
        static string TeamName => teamnames[rnd.Next(teamnames.Length - 1)];
        static string Country => countries[rnd.Next(countries.Length - 1)];
        static FSID Url => new(rnd.Next().ToString());

        readonly List<Match> matches;
        readonly int min;
        readonly int max;

        public MatchGenerator() { }
        public MatchGenerator(List<Match> matches, int min, int max)
        {
            this.matches = matches;
            this.min = min;
            this.max = max;
        }

        public Match Get()
        {
            throw new NotImplementedException();
            //return new(Url, Country, Country, t1, t2, DateTime.Now) { LastPersonalMatches = GetMatchesFromList() };
        }
        public List<Match> Get(int count)
        {
            var res = new List<Match>(count);
            if (count == 0) return res;

            for (int i = 0; i < count; i++)
                res.Add(Get());

            return res;
        }

        List<Match> GetMatchesFromList()
        {
            if (matches is null || matches.Count == 0 || min >= matches.Count) return null;

            var count = rnd.Next(Math.Max(min, 1), Math.Min(max, matches.Count));
            var res = new List<Match>(count);
            for (int i = 0; i < count; i++)
                res.Add(matches[rnd.Next(matches.Count - 1)]);
            return res;
        }
    }
}