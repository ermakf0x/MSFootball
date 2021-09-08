using FlashScore.Football.Parsing;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FlashScore.Football
{
    public static class FootballManager
    {
        static readonly FSFootballClient _client = FSFootballClient.Create();
        static readonly MatchCollection _currentMatches = new();
        static readonly object _sync = new();

        public static IReadOnlyList<Match> Matches => _currentMatches;

        public static async Task GetMatchesAsync(CancellationToken token)
        {
            if (_currentMatches.Count == 0)
            {
                var source = await _client.GetMatchesAsync(token);
                var collection = new RegularMatchesParser().Parse(source);
                //matches.AddRange(collection);
                //matches.AddRange(table.AddRange(collection));



                if (collection.Count > 0)
                {
                    _currentMatches.AddRange(collection);
                    //var temp = await LoadTempMatchesAsync();
                    //var md5Current = collection.GetMD5Hash();
                    //var md5Temp = temp.GetMD5Hash();
                    //currentMatches.AddRange(table.AddRange((md5Current != string.Empty && md5Current == md5Temp) ? temp : collection));
                }
            }
        }
        public static Task GetH2HAsync(IEnumerable<Match> matches, CancellationToken token, Action callback = null)
        {
            var parser = new RegularH2HParser();

            var blockAction = new ActionBlock<Match>(async match =>
            {
                var source = await _client.GetH2HAsync(match.Id, token);
                H2HParserResult res = null;

                lock (_sync) res = parser.Parse(source);

                match.LastPersonalMatches = res.LastPersonalMatches;
                match.FirstTeam.LastMatches = res.FirstTeamLastMatches;
                match.SecondTeam.LastMatches = res.SecondTeamLastMatches;
                match.Checked = true;

                callback?.Invoke();
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 5, CancellationToken = token });

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
                    var source = await _client.GetStatisticAsync(match.Id, token);
                    match.Statistic = parserStatistic.Parse(source);
                }
                if (match.Summary is null)
                {
                    var source = await _client.GetSummaryAsync(match.Id, token);
                    match.Summary = parserSummary.Parse(source);
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
    }
}
