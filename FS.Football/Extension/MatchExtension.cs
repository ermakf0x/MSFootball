using System.Collections.Generic;
using System.Linq;

namespace FS.Football.Extension
{
    public static class MatchExtension
    {
        public static IEnumerable<Match> GetCompletedMatches(this Match match, int limit = -1)
        {
            if (limit < 0)
            {
                var last = match.LastPersonalMatches ?? new MatchCollection();
                var first = match.FirstTeam.LastMatches ?? new MatchCollection();
                var second = match.SecondTeam.LastMatches ?? new MatchCollection();
                return last.Concat(first).Concat(second);
            }
            else
            {
                var last = match.LastPersonalMatches?.Take(limit) ?? new MatchCollection();
                var first = match.FirstTeam.LastMatches?.Take(limit) ?? new MatchCollection();
                var second = match.SecondTeam.LastMatches?.Take(limit) ?? new MatchCollection();
                return last.Concat(first).Concat(second);
            }
        }

        public static MatchDictionary ConcatMatchCollection(this MatchDictionary dict, IList<Match> matches, bool useSyncObject = false)
        {
            if (useSyncObject)
            {
                foreach (var match in matches)
                    lock (dict.SyncObject)
                        dict.TryAdd(match.Url, match);
            }
            else
            {
                foreach (var match in matches)
                    dict.TryAdd(match.Url, match);
            }

            return dict;
        }
    }
}
