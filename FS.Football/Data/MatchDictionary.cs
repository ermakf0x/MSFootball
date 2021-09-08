using System.Collections.Generic;

namespace FS.Football
{
    public class MatchDictionary : Dictionary<MatchUrl, Match>
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
