using FootballApp.Infrastructure.Logging;
using FootballApp.Infrastructure.Navigators;
using FlashScore.Football;
using System;
using System.Linq;
using System.Threading;
using FlashScore.Football.Utils;

namespace FootballApp.ViewModels
{
    class LoaderOfStatForMatchesViewModel : ViewModelBase
    {
        readonly INavigator navigator;
        readonly MatchesTableViewModel matchesTable;
        int h2hCount;
        int statAndSummCount;

        public int H2hCount { get => h2hCount; }
        public int StatAndSummCount
        {
            get => statAndSummCount;
            private set
            {
                statAndSummCount = value;
                OnPropertyChanged();
            }
        }

        public LoaderOfStatForMatchesViewModel(INavigator navigator, MatchesTableViewModel matchesTable)
        {
            this.navigator = navigator;
            this.matchesTable = matchesTable;
            LoadAsync();
            Global.FullStatLimit = 10;
        }

        async void LoadAsync()
        {
            try
            {
                var checkedMatches = matchesTable.Matches.Where(m => m.Selected);

                await FootballManager.GetH2HAsync(checkedMatches.Select(m => m.Match), CancellationToken.None, () =>
                {
                    Interlocked.Increment(ref h2hCount);
                    OnPropertyChanged(nameof(H2hCount));
                });

                ////////////////////////////////////////
                //navigator.NavigateTo(ViewType.MatchesTable);
                //return;
                ////////////////////////////////////////////

                var matches = new MatchCollection();
                foreach (var cm in checkedMatches)
                    matches.AddRange(cm.Match.GetCompletedMatches(Global.FullStatLimit));

                DeleteDuplicates(ref matches);

                var newMatches = new MatchCollection(matches.Count);
                foreach (var match in matches)
                {
                    if (match.Statistic != null && match.Summary != null) continue;

                    Match m = await FootballDataStorage.GetAsync(match.Id);
                    if (m != null)
                    {
                        match.Statistic = m.Statistic;
                        match.Summary = m.Summary;
                        StatAndSummCount++;
                    }
                    else
                    {
                        newMatches.Add(match);
                    }
                }

                if (newMatches.Count > 0)
                {
                    await FootballManager.GetSummaryAndStatisticAsync(newMatches, CancellationToken.None, () =>
                    {
                        Interlocked.Increment(ref statAndSummCount);
                        OnPropertyChanged(nameof(StatAndSummCount));
                    });

                    FootballDataStorage.Add(newMatches);
                    await FootballDataStorage.CommitAsync();
                }

                navigator.NavigateTo(ViewType.MatchesTable);
            }
            catch (Exception ex)
            {
                Logger.Send(ex.Message, Logger.MessageType.ERROR);
            }
        }

        static void DeleteDuplicates(ref MatchCollection matches)
        {
            var clearMatches = new MatchDictionary(matches.Count);
            foreach (var match in matches)
            {
                if (clearMatches.ContainsKey(match.Id)) continue;
                clearMatches.Add(match.Id, match);
            }

            var res = new MatchCollection(clearMatches.Count);
            res.AddRange(clearMatches.Values);
            matches = res;
        }
    }
}
