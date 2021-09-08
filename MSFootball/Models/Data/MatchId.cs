using System;

namespace MSFootball.Models.Data
{
    /// <summary>
    /// Уникальный идинтификатор футбольного матча
    /// </summary>
    [Serializable]
    public class MatchId
    {
        readonly string id;
        public MatchId(string url)
        {

            id = url;
        }

        public static bool operator ==(MatchId a, MatchId b)
        {
            return a.id == b.id;
        }
        public static bool operator !=(MatchId a, MatchId b)
        {
            return a.id != b.id;
        }
        public static bool operator ==(MatchId a, string str)
        {
            return a.id == str;
        }
        public static bool operator !=(MatchId a, string str)
        {
            return a.id != str;
        }
        public static bool operator ==(string str, MatchId a)
        {
            return str == a.id;
        }
        public static bool operator !=(string str, MatchId a)
        {
            return str != a.id;
        }

        public override string ToString()
        {
            return id;
        }
    }
}
