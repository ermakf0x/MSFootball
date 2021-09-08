using System;
using System.Text;

namespace FS.Football
{
    /// <summary> Уникальный адрес футбольного матча. </summary>
    [Serializable]
    public sealed class MatchUrl
    {
        readonly string url;
        public MatchUrl(string url)
        {
            // TODO: Проверить входной параметер url.
            this.url = url;
        }

        public static explicit operator MatchUrl(string str) => new MatchUrl(str);
        public static bool operator ==(MatchUrl a, MatchUrl b) => a.url == b.url;
        public static bool operator !=(MatchUrl a, MatchUrl b) => a.url != b.url;

        public byte[] ToBytes() => Encoding.UTF8.GetBytes(url);
        public override string ToString() => url;
        public override int GetHashCode() => url.GetHashCode();
        public override bool Equals(object obj)
        {
            if (obj is MatchUrl mId) return url.Equals(mId.url);
            return false;
        }
    }
}
