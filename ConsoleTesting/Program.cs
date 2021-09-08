using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ConsoleTesting
{
    static class Program
    {
        public static async Task Main()
        {
            //IDataStorage storage = new DefaultBinaryDataStorage();

            var matches = await MatchLoader.GetMatchesAsync();


            await MatchLoader.GetH2HAsync(matches.Take(100));
            var totalMatches = matches.SelectMany(m => m.FirstTeam.LastMatches?.Concat(m.SecondTeam.LastMatches ?? Enumerable.Empty<Match>())?.Concat(m.LastPersonalMatches ?? Enumerable.Empty<Match>()) ?? Enumerable.Empty<Match>())
                                      .ToList();
            TestDuplicate(totalMatches);
            Console.WriteLine($"\nTotal matches: {totalMatches.Count}");

            //using (var fs = File.Create("matches.bin"))
            //{
            //    MatchCollection.Serialize2(fs, MatchLoader.MatchCollection);
            //    fs.Position = 0;
            //    MatchCollection.Deserialize(fs);
            //}

            var totalCountries = totalMatches.Select(m => m.Country);
            TestDuplicate(totalCountries);

            //await MatchLoader.GetSummaryAsync(totalMatches.ToHashSet());

            //Console.WriteLine("Press key to save data");
            //Console.ReadKey();
            //storage.SaveData("Matches.dat", matches.ToList());

            Console.WriteLine("Press key to exit");
            Console.ReadKey();
        }

        static void TestDuplicate<T>(IEnumerable<T> items)
        {
            items.GroupBy(o => o)
                 .Take(2)
                 .Select(g => { Console.WriteLine("~~~~~~~~~~~~~"); return g; })
                 .SelectMany(g => g.Take(5))
                 .ForEach(a => Console.WriteLine(GetAddr(a)));
            Console.WriteLine("#############");
        }
        static void TestDuplicate<T, TOut>(IEnumerable<T> items, Func<T, TOut> func)
        {
            items.GroupBy(func)
                 .Take(2)
                 .Select(g => { Console.WriteLine("~~~~~~~~~~~~~"); return g; })
                 .SelectMany(g => g.Take(5))
                 .ForEach(a => Console.WriteLine(GetAddr(a)));
            Console.WriteLine("#############");
        }
        public static unsafe long GetAddr<T>(T obj)
        {
            TypedReference tr = __makeref(Unsafe.AsRef(obj));
            IntPtr ptr = **(IntPtr**)(&tr);
            return ptr.ToInt64();
        }
        static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items is null) return;
            foreach (var item in items) action(item);
        }
    }
}