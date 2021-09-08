using System;
using System.Collections.Generic;

namespace ConsoleTesting
{
    class SummaryParser : FS.Football.Parsing.IDataParser<MatchSummary>
    {
        public MatchSummary Parse(string source)
        {
            if (string.IsNullOrEmpty(source)) return null;

            IReadOnlyCollection<MatchSummary.Event> first = null;
            IReadOnlyCollection<MatchSummary.Event> second = null;

            try
            {
                var s = source.Split(new string[] { "AC÷" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var a in s)
                {
                    if (a.Contains("1-й тайм")) first = GetHalfSummary(a);
                    else if (a.Contains("2-й тайм")) second = GetHalfSummary(a);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(source + "\n");
            }

            return new MatchSummary(first, second);
        }

        static IReadOnlyCollection<MatchSummary.Event> GetHalfSummary(string source)
        {
            var res = new List<MatchSummary.Event>();
            var temp = source.Split(new string[] { "~III÷" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < temp.Length; i++)
            {
                var temp2 = temp[i].Split('¬');

                var teamID = byte.Parse(temp2[1][3..]);
                var timeStr = temp2[2][3..^1];
                var typeNumber = int.Parse(temp2[3][3..]);
                var name = temp2[4][3..];

                byte time;
                if (timeStr.Contains("+"))
                {
                    var t = timeStr.Split('+');
                    time = (byte)(byte.Parse(t[0]) + byte.Parse(t[1]));
                }
                else time = byte.Parse(timeStr);

                res.Add(new(time, teamID, (MatchSummary.EventType)typeNumber, name));
            }

            return res;
        }
    }
}
