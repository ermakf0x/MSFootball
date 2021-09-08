using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ConsoleTesting
{
    static class MatchLoader
    {
        public static async Task<List<Match>> GetMatchesAsync()
        {
            using var client = FS.Football.FSFootballClient.Create();
            var source = await client.GetMatchesAsync(default);
            var res = new MatchesParser().Parse(source);
            Console.WriteLine($"Loaded {res.Count} matches.");
            return res;
        }
        public static async Task GetH2HAsync(IEnumerable<Match> matches)
        {
            using var client = FS.Football.FSFootballClient.Create();
            var parser = new H2HParser();
            var key = new object();

            var blockAction = new ActionBlock<Match>(async match =>
            {
                var source = await client.GetH2HAsync(match.Id.ToString(), default);
                H2HParser.Result res = null;
                lock (key) res = parser.Parse(source);
                match.LastPersonalMatches = res.Personal.ToList();
                match.FirstTeam.LastMatches = res.First.ToList();
                match.SecondTeam.LastMatches = res.Second.ToList();
                //Console.WriteLine(res);
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 20 });


            foreach (var match in matches)
                blockAction.Post(match);

            blockAction.Complete();
            await blockAction.Completion;
        }
        public static async Task GetSummaryAsync(IEnumerable<Match> matches)
        {
            using var client = FS.Football.FSFootballClient.Create();
            var parser = new SummaryParser();

            var blockAction = new ActionBlock<Match>(async match =>
            {
                var source = await client.GetSummaryAsync(match.Id.ToString(), default);
                match.Summary = parser.Parse(source);

            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 20 });


            foreach (var match in matches)
                if (match.Summary is null) blockAction.Post(match);

            blockAction.Complete();
            await blockAction.Completion;
        }


        //public static Task GetStatisticAsync(IEnumerable<Match> matches, CancellationToken token, Action callback = null)
        //{
        //    var parserStatistic = new RegularStatisticParser();
        //    var parserSummary = new RegularSummaryParser();

        //    var blockAction = new ActionBlock<Match>(async match =>
        //    {
        //        if (match.Statistic is null)
        //        {
        //            match.Statistic = await _client.GetStatisticAsync(match.Url, token, parserStatistic);
        //        }
        //        if (match.Summary is null)
        //        {
        //            match.Summary = await _client.GetSummaryAsync(match.Url, token, parserSummary);
        //        }

        //        callback?.Invoke();
        //    }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 20, CancellationToken = token });


        //    foreach (var match in matches)
        //    {
        //        if (match.Statistic is null || match.Summary is null)
        //            blockAction.Post(match);
        //        else callback?.Invoke();
        //    }
        //    blockAction.Complete();
        //    return blockAction.Completion;
        //}
    }
}