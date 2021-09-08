using MSFootball.Models.Data;
using MSFootball.Models.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MSFootball.Models
{
    static class FootballManager
    {
        static readonly object keyLock = new object();
        static DateTime timeOfCreate;
        static readonly Dictionary<MatchId, EndedMatch> endedMatches;
        static readonly List<CurrentMatch> matches;
        public static IReadOnlyList<CurrentMatch> Matches => matches;

        static FootballManager()
        {
            matches = new List<CurrentMatch>();
            endedMatches = new Dictionary<MatchId, EndedMatch>();
        }

        public static Task GetMatchesAsync()
        {
            if (matches.Count == 0)
            {
                return MSClient.Instance.GetDataAsync("/x/feed/f_1_0_2_cs_1_0").ContinueWith(t =>
                {
                    timeOfCreate = DateTime.UtcNow;
                    var res = new RegularMatchesParser().Parse(t.Result);
                    if (res.Count > 0)
                        matches.AddRange(res);
                });
            }
            return Task.CompletedTask;
        }
        public static Task GetH2HAsync(IEnumerable<CurrentMatch> matches, CancellationToken token, Action updateCallBack = null)
        {
            IDataParser<H2HParserResult> parser = new RegularH2HParser();

            var blockAction = new ActionBlock<CurrentMatch>(async match =>
            {
                var source = await MSClient.Instance.GetDataAsync("/x/feed/df_hh_1_" + match.Id);
                var res = parser.Parse(source);

                CheckMatches(res.FirstTeamLastMatches);
                CheckMatches(res.SecondTeamLastMatches);
                if (res.LastPersonalMatches != null)
                    CheckMatches(res.LastPersonalMatches);

                match.LastPersonalMatches = res.LastPersonalMatches;
                match.FirstTeam.LastMatches = res.FirstTeamLastMatches;
                match.SecondTeam.LastMatches = res.SecondTeamLastMatches;
                match.Checked = true;

                updateCallBack?.Invoke();
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 10, CancellationToken = token });

            foreach (var match in matches)
            {
                if (!match.Checked)
                    blockAction.Post(match);
                else updateCallBack?.Invoke();
            }
            blockAction.Complete();
            return blockAction.Completion;
        }
        public static Task GetSummaryAndStatisticAsync(IEnumerable<EndedMatch> matches, CancellationToken token, Action updateCallBack = null)
        {
            IDataParser<MatchStatistic> parserStatistic = new RegularStatisticParser();
            IDataParser<MatchSummary> parserSummary = new RegularSummaryParser();

            var blockAction = new ActionBlock<EndedMatch>(async match =>
            {
                if(match.Statistic == null)
                {
                    var source = await MSClient.Instance.GetDataAsync("/x/feed/df_st_1_" + match.Id);
                    match.Statistic = parserStatistic.Parse(source);
                }
                if (match.Summary == null)
                {
                    var source = await MSClient.Instance.GetDataAsync("/x/feed/df_su_1_" + match.Id);
                    match.Summary = parserSummary.Parse(source);
                }

                updateCallBack?.Invoke();
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 10, CancellationToken = token });


            foreach (var match in matches)
            {
                if (match.Statistic == null || match.Summary == null)
                    blockAction.Post(match);
                else updateCallBack?.Invoke();
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
                    if (!File.Exists("Temp"))
                        Directory.CreateDirectory("Temp");

                    using (var fs = new FileStream("Temp\\Matches.dat", FileMode.Create))
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(fs, matches);
                    }
                    File.SetCreationTimeUtc("Temp\\Matches.dat", timeOfCreate);
                }
                catch { }
            });
        }
        public static Task LoadTempMatchesAsync()
        {
            if (!File.Exists("Temp\\Matches.dat")) return Task.CompletedTask;

            timeOfCreate = File.GetCreationTimeUtc("Temp\\Matches.dat");
            if (timeOfCreate.Date != DateTime.UtcNow.Date) return Task.CompletedTask;

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var fs = new FileStream("Temp\\Matches.dat", FileMode.Open))
                    {
                        var formatter = new BinaryFormatter();
                        matches.AddRange(formatter.Deserialize(fs) as List<CurrentMatch>);
                    }

                    foreach (var match in matches)
                    {
                        if (match.LastPersonalMatches != null)
                            CheckMatches(match.LastPersonalMatches);
                        if (match.FirstTeam.LastMatches != null)
                            CheckMatches(match.FirstTeam.LastMatches);
                        if (match.SecondTeam.LastMatches != null)
                            CheckMatches(match.SecondTeam.LastMatches);
                    }
                }
                catch
                {
                    matches.Clear();
                    endedMatches.Clear();
                }
            });
        }

        static void CheckMatches(List<EndedMatch> endedMatches)
        {
            for (int i = 0; i < endedMatches.Count; i++)
            {
                lock (keyLock)
                {
                    if (FootballManager.endedMatches.ContainsKey(endedMatches[i].Id))
                        endedMatches[i] = FootballManager.endedMatches[endedMatches[i].Id];
                    else
                        FootballManager.endedMatches.Add(endedMatches[i].Id, endedMatches[i]);
                }
            }
        }
    }
}
