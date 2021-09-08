using FS.Football.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FS.Football
{
    public static class FootballManager
    {
        static readonly FSFootballClient _client;
        static readonly MatchCollection matches;
        static readonly FootballMatchTable table;
        static readonly object sync = new();

        public static IReadOnlyList<Match> Matches => matches;

        static FootballManager()
        {
            _client = FSFootballClient.Create();

            matches = new MatchCollection();
            table = new();
        }

        public static async Task GetMatchesAsync(CancellationToken token)
        {
            if (matches.Count == 0)
            {
                var source = await _client.GetMatchesAsync(token);
                var collection = new RegularMatchesParser().Parse(source);
                //matches.AddRange(collection);
                //matches.AddRange(table.AddRange(collection));



                if (collection.Count > 0)
                {
                    var temp = await LoadTempMatchesAsync();
                    var md5Current = collection.GetMD5Hash();
                    var md5Temp = temp.GetMD5Hash();
                    matches.AddRange(table.AddRange((md5Current != string.Empty && md5Current == md5Temp) ? temp : collection));
                }
            }
        }
        public static Task GetH2HAsync(IEnumerable<Match> matches, CancellationToken token, Action callback = null)
        {
            var parser = new RegularH2HParser();

            var blockAction = new ActionBlock<Match>(async match =>
            {
                var res = await _client.GetH2HAsync(match.Url, token, parser);

                lock (sync)
                {
                    match.LastPersonalMatches = table.AddRange(res.LastPersonalMatches);
                    match.FirstTeam.LastMatches = table.AddRange(res.FirstTeamLastMatches);
                    match.SecondTeam.LastMatches = table.AddRange(res.SecondTeamLastMatches);
                }
                match.Checked = true;

                callback?.Invoke();
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 20, CancellationToken = token });

            foreach (var match in matches)
            {
                if (!match.Checked)
                    blockAction.Post(match);
                else callback?.Invoke();
            }
            blockAction.Complete();
            return blockAction.Completion;
        }
        public static Task GetSummaryAndStatisticAsync(IEnumerable<Match> matches, CancellationToken token, Action callback = null)
        {
            var parserStatistic = new RegularStatisticParser();
            var parserSummary = new RegularSummaryParser();

            var blockAction = new ActionBlock<Match>(async match =>
            {
                if (match.Statistic is null)
                {
                    match.Statistic = await _client.GetStatisticAsync(match.Url, token, parserStatistic);
                }
                if (match.Summary is null)
                {
                    match.Summary = await _client.GetSummaryAsync(match.Url, token, parserSummary);
                }

                callback?.Invoke();
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 20, CancellationToken = token });


            foreach (var match in matches)
            {
                if (match.Statistic is null || match.Summary is null)
                    blockAction.Post(match);
                else callback?.Invoke();
            }
            blockAction.Complete();
            return blockAction.Completion;
        }

        public static Task SaveTempMatchesAsync()
        {
            if (matches.Count == 0) return Task.CompletedTask;

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!File.Exists("Temp")) Directory.CreateDirectory("Temp");

                    using var fs = new FileStream("Temp\\Matches.dat", FileMode.Create);
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(fs, matches);
                }
                catch(Exception ex)
                {

                }
            });
        }
        public static Task<MatchCollection> LoadTempMatchesAsync()
        {
            if (!File.Exists("Temp\\Matches.dat")) return Task.FromResult(new MatchCollection());

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    using var fs = new FileStream("Temp\\Matches.dat", FileMode.Open);
                    var formatter = new BinaryFormatter();
                    return formatter.Deserialize(fs) as MatchCollection;
                }
                catch(Exception ex)
                {
                    return new MatchCollection();
                }
            });
        }
    }
}
