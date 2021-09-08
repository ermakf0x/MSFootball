using FS.Core.Serialization;
using FS.Football.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FS.Football
{
    [Serializable]
    public class Team : ISerializable
    {
        public string Name { get; }
        public IList<Match> LastMatches { get; set; }
        public Team(string name)
        {
            Name = Helper.HasNullOrWhiteSpace(name);
        }

        public static bool operator ==(Team ft, Team st) => ft.Name == st.Name;
        public static bool operator !=(Team ft, Team st) => !(ft.Name == st.Name);

        public override bool Equals(object obj)
        {
            if (obj is Team t) return Name.Equals(t.Name);
            return false;
        }
        public override int GetHashCode() => Name.GetHashCode();
        public override string ToString() => Name;

        #region Serializable

        Team(SerializationInfo info, StreamingContext context)
        {
            SerializationReader r = SerializationReader.GetReader(info);
            Name = r.ReadString();
            var list = r.ReadList<Match>();
            if (list != null) LastMatches = new MatchCollection(list);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter w = SerializationWriter.GetWriter();
            w.Write(Name);
            w.Write(LastMatches);
            w.AddToInfo(info);
        }

        #endregion
    }
}
