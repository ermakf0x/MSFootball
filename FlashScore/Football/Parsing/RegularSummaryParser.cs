using System;

namespace FlashScore.Football.Parsing
{
    public class RegularSummaryParser : IDataParser<MatchSummary>
    {
        public MatchSummary Parse(string source)
        {
            if (string.IsNullOrEmpty(source)) return MatchSummary.Empty;

            Summary first = null;
            Summary second = null;

            try
            {
                var s = source.Split(new string[] { "AC÷" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var a in s)
                {
                    if (a.Contains("1-й тайм")) first = GetSummary(a);
                    else if (a.Contains("2-й тайм")) second = GetSummary(a);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(source + "\n");
            }

            return new MatchSummary(first, second);
        }
        Summary GetSummary(string source)
        {
            var res = new Summary();
            var temp = source.Split(new string[] { "~III÷" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < temp.Length; i++)
            {
                var temp2 = temp[i].Split('¬');

                var teamID =        byte.Parse(temp2[1].Substring(3));
                var timeStr =       temp2[2].Substring(3, temp2[2].Length - 4);
                var typeNumber =    int.Parse(temp2[3].Substring(3));
                var name =          temp2[4].Substring(3);

                byte time;
                if (timeStr.Contains("+"))
                {
                    var t = timeStr.Split('+');
                    time = (byte)(byte.Parse(t[0]) + byte.Parse(t[1]));
                }
                else time = byte.Parse(timeStr);

                res.Add(new MatchEvent(time, teamID, (MatchEventType)typeNumber, name));
            }

            if (res.Count == 0)
                return null;
            return res;
        }
    }
}
