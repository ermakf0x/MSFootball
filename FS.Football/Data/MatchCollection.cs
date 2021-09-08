using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FS.Football
{
    /// <summary> Коллекция футбольных матчей. </summary>
    [Serializable]
    public sealed class MatchCollection : List<Match>
    {
        public MatchCollection() { }
        public MatchCollection(int capacity) : base(capacity) { }
        public MatchCollection(IEnumerable<Match> collection) : base(collection) { }

        public string GetMD5Hash()
        {
            if (Count == 0) return string.Empty;

            var data = new List<byte>(Count * 4);
            foreach (var match in this)
                data.AddRange(match.Url.ToBytes());            

            var md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(data.ToArray());

            var sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
