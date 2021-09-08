using MSFootball.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace MSFootball.Models
{
    public class FootballDataAccess: IDisposable
    {
        const string path = "Football.fdb";
        const string connectionString = "Data Source=" + path + "; Version=3;";

        SQLiteConnection connect;
        Dictionary<MatchId, EndedMatch> newMatches;

        public static FootballDataAccess OpenDataBase()
        {
            if (!File.Exists(path))
                SQLiteConnection.CreateFile(path);

            var connect = new SQLiteConnection(connectionString);
            var command = new SQLiteCommand
            {
                Connection = connect,
                CommandText = "CREATE TABLE IF NOT EXISTS dbTableName (id VARCHAR(8) PRIMARY KEY NOT NULL, " +
                "country NVARCHAR(64), championship NVARCHAR(64), time INTEGER, team1 NVARCHAR(64), " +
                "team2 NVARCHAR(64), score SMALLINT, statistic BINARY, summary BINARY)"
            };
            connect.Open();
            command.ExecuteNonQuery();

            return new FootballDataAccess(connect);
        }

        FootballDataAccess(SQLiteConnection connect)
        {
            this.connect = connect;
            newMatches = new Dictionary<MatchId, EndedMatch>();
        }

        public void Add(EndedMatch match)
        {
            if(match != null && !newMatches.ContainsKey(match.Id))
            {
                newMatches.Add(match.Id, match);
            }
        }
        public void AddRange(List<EndedMatch> matches)
        {
            if(matches != null && matches.Count > 0)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    Add(matches[i]);
                }
            }
        }
        public EndedMatch GetMatch(MatchId id)
        {
            if (id is null) return null;
            EndedMatch res = null;

            using(var command = new SQLiteCommand(connect))
            {
                command.CommandText = $"SELECT * FROM dbTableName WHERE id = '{id}'";

                var sqlReader = command.ExecuteReader();
                if (sqlReader.Read())
                {
                    res = ReadMatch(sqlReader);
                }
                sqlReader.Close();

                return res;
            }
        }
        public List<EndedMatch> GetMatches(List<MatchId> ids)
        {
            if (ids is null || ids.Count == 0) return null;
            var res = new List<EndedMatch>();
            var dic = new Dictionary<MatchId, EndedMatch>(ids.Count);

            using(var command = new SQLiteCommand(connect))
            {
                command.CommandText = $"SELECT * FROM dbTableName";
                var sqlReader = command.ExecuteReader();
                while(sqlReader.Read())
                {
                    var m = ReadMatch(sqlReader);
                    if (!dic.ContainsKey(m.Id))
                    {
                        dic.Add(m.Id, m);
                    }
                }
                sqlReader.Close();

                EndedMatch match = null;
                foreach (var id in ids)
                {
                    if(dic.TryGetValue(id, out match))
                    {
                        res.Add(match);
                    }
                }
            }

            return res;
        }
        public void SaveChanges()
        {
            if (newMatches.Count == 0) return;

            using (var command = new SQLiteCommand(connect))
            {
                command.CommandText = "INSERT INTO dbTableName (id, country, championship, time, team1, team2, score, statistic, summary) " +
                                      "VALUES(@id, @country, @championship, @time, @team1, @team2, @score, @statistic, @summary) " +
                                      "ON CONFLICT(id) DO UPDATE SET id=excluded.id";

                using (var transaction = connect.BeginTransaction())
                {
                    foreach (var match in newMatches)
                    {
                        if (match.Value.Statistic == null || match.Value.Summary == null) continue;

                        AddMatchToCommand(command, match.Value);
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            newMatches.Clear();
        }
        public void Dispose()
        {
            connect.Close();
            connect.Dispose();
        }

        void AddMatchToCommand(SQLiteCommand command, EndedMatch match)
        {
            command.Parameters.AddWithValue("@id", match.Id.ToString());
            command.Parameters.AddWithValue("@country", match.Country);
            command.Parameters.AddWithValue("@championship", match.Championship);
            command.Parameters.AddWithValue("@time", match.Time.ToBinary());
            command.Parameters.AddWithValue("@team1", match.TeamName1);
            command.Parameters.AddWithValue("@team2", match.TeamName2);
            command.Parameters.AddWithValue("@score", match.Score?.ToBinary());
            command.Parameters.AddWithValue("@statistic", match.Statistic?.ToBinary());
            command.Parameters.AddWithValue("@summary", match.Summary?.ToBinary());
        }
        EndedMatch ReadMatch(SQLiteDataReader reader)
        {
            var id = reader["id"].ToString();
            var country = reader["country"].ToString();
            var championship = reader["championship"].ToString();
            var team1 = reader["team1"].ToString();
            var team2 = reader["team2"].ToString();
            var time = DateTime.FromBinary((long)reader["time"]);

            ScoreMatch scoreMatch = null;
            if (reader["score"] is short score)
                scoreMatch = ScoreMatch.ToObject(score);

            MatchStatistic statistic = null;
            if (reader["statistic"] is byte[] statBytes)
                statistic = MatchStatistic.ToObject(statBytes);

            MatchSummary summary = null;
            if (reader["summary"] is byte[] summBytes)
                summary = MatchSummary.ToObject(summBytes);

            return new EndedMatch(new MatchId(id), country, championship, time, team1, team2, scoreMatch)
            {
                Statistic = statistic,
                Summary = summary
            };
        }
    }
}
