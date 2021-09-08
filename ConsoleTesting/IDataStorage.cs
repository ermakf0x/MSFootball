using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace ConsoleTesting
{
    interface IDataStorage
    {
        void SaveData(string path, List<Match> matches);
        List<Match> LoadData(string path);
    }

    class DefaultBinaryDataStorage : IDataStorage
    {
        public List<Match> LoadData(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(fs) as List<Match>;
        }

        public void SaveData(string path, List<Match> matches)
        {
            using var fs = new FileStream(path, FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(fs, matches);
        }
    }

    class XmlDataStorage : IDataStorage
    {
        public List<Match> LoadData(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            var xs = new XmlSerializer(typeof(List<Match>));
            return xs.Deserialize(fs) as List<Match>;
        }

        public void SaveData(string path, List<Match> matches)
        {
            using var fs = new FileStream(path, FileMode.Create);
            var xs = new XmlSerializer(typeof(List<Match>));
            xs.Serialize(fs, matches);
        }
    }
}