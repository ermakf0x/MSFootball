using System;
using System.Text;

namespace FlashScore
{
    [Serializable]
    public readonly struct FSID : IEquatable<FSID>
    {
        readonly long _id;

        public FSID(string strId)
        {
            ReadOnlySpan<char> chars = strId;
            Span<byte> bytes = stackalloc byte[8];
            Encoding.ASCII.GetBytes(chars, bytes);
            _id = BitConverter.ToInt64(bytes);
        }
        public FSID(long id) => _id = id;

        public byte[] ToBinary() => BitConverter.GetBytes(_id);
        public long ToInt64() => _id;

        public override int GetHashCode() => _id.GetHashCode();
        public override string ToString() => Encoding.ASCII.GetString(BitConverter.GetBytes(_id));
        public override bool Equals(object obj)
        {
            if (obj is FSID u) return _id.Equals(u._id);
            return false;
        }
        public bool Equals(FSID other) => _id == other._id;
        public static bool operator ==(FSID left, FSID right) => left._id == right._id;
        public static bool operator !=(FSID left, FSID right) => !(left._id == right._id);
    }
}
