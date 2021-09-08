using DevExpress.Mvvm;
using MSFootball.Models;
using MSFootball.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MSFootball.ViewModels
{
    class LoaderEndedMatchesVM : BindableBase
    {
        public int h2hCount { get; set; }
        public int StatAndSummCount { get; set; }
        public ICommand LoadAsync { get; }

        public LoaderEndedMatchesVM()
        {
            LoadAsync = new AsyncCommand(_LoadAsync);
        }

        async Task _LoadAsync()
        {
            try
            {
                if (FootballManager.Matches.Count == 0) return;

                var checkedMatches = FootballManager.Matches.Where(m => m.Selected);

                await FootballManager.GetH2HAsync(checkedMatches, CancellationToken.None, () =>
                {
                    h2hCount++;
                    RaisePropertyChanged("h2hCount");
                });

                var matches = new List<EndedMatch>();
                foreach (var cm in checkedMatches)
                    matches.AddRange(cm.GetAllEndedMatches(Global.Limit));

                DeleteDuplicates(ref matches);

                var newMatches = new List<EndedMatch>(matches);
                //using (var db = FootballDataAccess.OpenDataBase())
                //{
                //    foreach (var match in matches)
                //    {
                //        if (match.Statistic == null || match.Summary == null)
                //        {
                //            var m = db.GetMatch(match.Id);
                //            if (m != null)
                //            {
                //                match.Statistic = m.Statistic;
                //                match.Summary = m.Summary;
                //                RaisePropertyChanged("StatAndSummCount");
                //            }
                //            else
                //            {
                //                newMatches.Add(match);
                //            }
                //        }
                //        StatAndSummCount++;
                //        RaisePropertyChanged("StatAndSummCount");
                //    }
                //}

                if (newMatches.Count > 0)
                {
                    await FootballManager.GetSummaryAndStatisticAsync(newMatches, CancellationToken.None, () =>
                    {
                        StatAndSummCount++;
                        RaisePropertyChanged("StatAndSummCount");
                    });

                    await FootballManager.SaveTempMatchesAsync();

                    using(var db = FootballDataAccess.OpenDataBase())
                    {
                        db.AddRange(newMatches);
                        db.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        void DeleteDuplicates(ref List<EndedMatch> matches)
        {
            var clearMatches = new Dictionary<MatchId, EndedMatch>(matches.Count);
            foreach (var match in matches)
            {
                if (clearMatches.ContainsKey(match.Id)) continue;
                clearMatches.Add(match.Id, match);
            }

            var res = new List<EndedMatch>(clearMatches.Count);
            res.AddRange(clearMatches.Values);
            matches = res;
        }
    }
}
