using System;
using System.Text;

namespace ConsoleTesting
{
    [Serializable]
    public readonly struct FSID
    {
        readonly long id;

        public FSID(string url)
        {
            var bytes = Encoding.ASCII.GetBytes(url);
            id = BitConverter.ToInt64(bytes);
        }

        public override bool Equals(object obj)
        {
            if (obj is FSID u) return id.Equals(u.id);
            return false;
        }
        public override int GetHashCode() => id.GetHashCode();
        public override string ToString() => Encoding.ASCII.GetString(BitConverter.GetBytes(id));
        public byte[] ToBinary() => BitConverter.GetBytes(id);
        public static bool operator ==(FSID left, FSID right) => left.id == right.id;
        public static bool operator !=(FSID left, FSID right) => !(left.id == right.id);
    }
}