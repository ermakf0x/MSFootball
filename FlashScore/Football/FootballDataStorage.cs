using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace FlashScore.Football
{
    public static class FootballDataStorage
    {
        const string _path = "Football.fdb";
        const string _dbTableName = "dbTableName";
        static readonly Dictionary<FSID, Match> _matches = new();

        public static async Task<Match> GetAsync(FSID id)
        {
            using var connect = await OpenAsync();
            using var command = new SQLiteCommand($"SELECT * FROM {_dbTableName} WHERE id = '{id.ToInt64()}'", connect);
            using var reader = await command.ExecuteReaderAsync();
            if (reader.Read()) return ReadMatch((SQLiteDataReader)reader);
            return null;
        }
        public static async Task<List<Match>> GetAsync(IEnumerable<FSID> ids)
        {
            var res = new List<Match>();
            using var connect = await OpenAsync();
            using var command = new SQLiteCommand(connect);

            foreach (var id in ids)
            {
                command.CommandText = $"SELECT * FROM {_dbTableName} WHERE id = '{id.ToInt64()}'";
                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read()) res.Add(ReadMatch((SQLiteDataReader)reader));
            }

            return res;
        }
        public static async Task<bool> CommitAsync()
        {
            if (_matches.Count == 0) return true;

            var commandText = $"INSERT INTO {_dbTableName} (id, country_id, championship, time, team1, team2, score, statistic, summary) " +
                               "VALUES(@id, @country, @championship, @time, @team1, @team2, @score, @statistic, @summary) " +
                               "ON CONFLICT(id) DO UPDATE SET id=excluded.Id";

            using var connect = await OpenAsync();
            using var command = new SQLiteCommand(commandText, connect);
            using var transaction = await connect.BeginTransactionAsync();
            foreach (var match in _matches)
            {
                AddToCommand(command, match.Value);
                await command.ExecuteNonQueryAsync();
            }
            await transaction.CommitAsync();
            _matches.Clear();
            return true;
        }

        public static void Add(Match match)
        {
            if (match is null || match.Statistic == null || match.Summary == null) return;

            if (_matches.ContainsKey(match.Id)) _matches[match.Id] = match;
            else _matches.Add(match.Id, match);
        }
        public static void Add(IEnumerable<Match> matches)
        {
            foreach (var match in matches)
                Add(match);
        }
        
        static async Task<SQLiteConnection> OpenAsync()
        {
            if (!File.Exists(_path)) SQLiteConnection.CreateFile(_path);

            var connect = new SQLiteConnection("Data Source=" + _path + "; Version=3;");
            await connect.OpenAsync();

            var command = new SQLiteCommand
            {
                Connection = connect,
                CommandText = "PRAGMA foreign_keys=on;" +
                $"CREATE TABLE IF NOT EXISTS {_dbTableName} (id INTEGER PRIMARY KEY NOT NULL, country_id INTEGER NOT NULL, championship NVARCHAR(64), " +
                "time DATATIME, team1 NVARCHAR(64), team2 NVARCHAR(64), score BINARY, statistic BINARY, summary BINARY, FOREIGN KEY(country_id) REFERENCES countries(id));" +
                "CREATE TABLE IF NOT EXISTS countries (id INTEGER PRIMARY KEY, name NVARCHAR(64));"
            };
            await command.ExecuteNonQueryAsync();
            return connect;
        }
        static Match ReadMatch(SQLiteDataReader reader)
        {
            var id = (long)reader["id"];
            var country = reader["country"].ToString();
            var championship = reader["championship"].ToString();
            var teamName1 = reader["team1"].ToString();
            var teamName2 = reader["team2"].ToString();
            var time = DateTime.Parse(reader["time"].ToString());

            ScoreMatch scoreMatch = ScoreMatch.Empty;
            if (reader["score"] is byte[] score)
                scoreMatch = ScoreMatch.ToObject(score);

            MatchStatistic statistic = MatchStatistic.Empty;
            if (reader["statistic"] is byte[] statBytes)
                statistic = MatchStatistic.ToObject(statBytes);

            MatchSummary summary = MatchSummary.Empty;
            if (reader["summary"] is byte[] summBytes)
                summary = MatchSummary.ToObject(summBytes);

            var team1 = FootballMatchSet.NewTeam(teamName1);
            var team2 = FootballMatchSet.NewTeam(teamName2);
            var match = FootballMatchSet.NewMatch(new(id), country, championship, team1, team2, time);
            match.Score = scoreMatch;
            match.Statistic = statistic;
            match.Summary = summary;
            return match;
        }
        static void AddToCommand(SQLiteCommand command, Match match)
        {
            command.Parameters.AddWithValue("@id", match.Id.ToInt64());
            command.Parameters.AddWithValue("@country", match.Country);
            command.Parameters.AddWithValue("@championship", match.Championship);
            command.Parameters.AddWithValue("@time", match.Time);
            command.Parameters.AddWithValue("@team1", match.FirstTeam.Name);
            command.Parameters.AddWithValue("@team2", match.SecondTeam.Name);
            command.Parameters.AddWithValue("@score", match.Score?.ToBinary());
            command.Parameters.AddWithValue("@statistic", match.Statistic?.ToBinary());
            command.Parameters.AddWithValue("@summary", match.Summary?.ToBinary());
        }
    }
}
