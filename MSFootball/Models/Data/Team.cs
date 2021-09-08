using System;
using System.Collections.Generic;

namespace MSFootball.Models.Data
{
    /// <summary>
    /// Футбольная команда
    /// </summary>
    [Serializable]
    class Team
    {
        /// <summary>
        /// Имя команды
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Следующий матч команды
        /// </summary>
        public CurrentMatch NextMatch { get; set; }
        /// <summary>
        /// Последние матчи команды
        /// </summary>
        public List<EndedMatch> LastMatches { get; set; }

        public Team(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
