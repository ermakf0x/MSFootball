using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MSFootball.Models.Data
{
    /// <summary>
    /// Футбольный матч, который должен быть сыгран.
    /// </summary>
    [Serializable]
    class CurrentMatch : MatchBase, INotifyPropertyChanged
    {
        [NonSerialized]
        private bool selected;

        /// <summary>
        /// Происходит при изминении свойства IsChecked
        /// </summary>
        [field: NonSerialized]
        public event Action<CurrentMatch> SelectedChanged;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Отмечен текучий матч или нет
        /// </summary>
        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Selected"));
                SelectedChanged?.Invoke(this);
            }
        }
        public bool Checked { get; set; }

        /// <summary>
        /// Первая команда
        /// </summary>
        public Team FirstTeam { get; set; }
        /// <summary>
        /// Вторая команда
        /// </summary>
        public Team SecondTeam { get; set; }
        /// <summary>
        /// Очные встречи двух команд
        /// </summary>
        public List<EndedMatch> LastPersonalMatches { get; set; }

        public CurrentMatch(MatchId id, string country, string championship, DateTime time, Team first, Team second) : base(id, country, championship, time)
        {
            FirstTeam = first ?? throw new ArgumentNullException(nameof(first));
            SecondTeam = second ?? throw new ArgumentNullException(nameof(second));
            FirstTeam.NextMatch = SecondTeam.NextMatch = this;
        }

        public List<EndedMatch> GetAllEndedMatches(int limit)
        {
            if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit), "Не может быть меньше 0.");

            int capPersonal = 0, capFirst = 0, capSecond = 0;
            if (LastPersonalMatches != null)
                capPersonal = Math.Min(LastPersonalMatches.Count, limit);
            if (FirstTeam.LastMatches != null)
                capFirst = Math.Min(FirstTeam.LastMatches.Count, limit);
            if (SecondTeam.LastMatches != null)
                capSecond = Math.Min(SecondTeam.LastMatches.Count, limit);
            var res = new List<EndedMatch>(capPersonal + capFirst + capSecond);

            if (LastPersonalMatches != null)
                for (int i = 0; i < capPersonal; i++)
                    res.Add(LastPersonalMatches[i]);
            if (FirstTeam.LastMatches != null)
                for (int i = 0; i < capFirst; i++)
                    res.Add(FirstTeam.LastMatches[i]);
            if (SecondTeam.LastMatches != null)
                for (int i = 0; i < capSecond; i++)
                    res.Add(SecondTeam.LastMatches[i]);

            return res;
        }
        public override string ToString()
        {
            return $"{FirstTeam} - {SecondTeam}";
        }
    }
}
