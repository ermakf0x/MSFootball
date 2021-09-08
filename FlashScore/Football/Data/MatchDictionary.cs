using System.Collections.Generic;

namespace FlashScore.Football
{
    public class MatchDictionary : Dictionary<FSID, Match>
    {
        public object SyncObject { get; } = new object();

        public MatchDictionary() { }
        public MatchDictionary(int capacity) : base(capacity) { }

        public MatchCollection ToCollection()
        {
            return new MatchCollection(Values);
        }
    }
}
