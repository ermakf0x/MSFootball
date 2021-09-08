using System;

namespace MSFootball.Models.Data
{
    /// <summary>
    /// Абстрактный класс футбольного матча
    /// </summary>
    [Serializable]
    public abstract class MatchBase
    {
        /// <summary>
        /// Ссылка на матч
        /// </summary>
        public MatchId Id { get; }
        /// <summary>
        /// Страна матча
        /// </summary>
        public string Country { get; }
        /// <summary>
        /// Чемпионат матча
        /// </summary>
        public string Championship { get; }
        /// <summary>
        /// Время матча
        /// </summary>
        public DateTime Time { get; }        

        public MatchBase(MatchId id, string country, string championship, DateTime time)
        {
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Championship = championship ?? throw new ArgumentNullException(nameof(championship));
            Time = time;
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        /// <summary>
        /// Парсит дату начиная с 1970.1.1
        /// </summary>
        /// <param name="seconds">Время в секундах</param>
        /// <returns></returns>
        public static DateTime ParseDT(int seconds)
        {
            return new DateTime(1970, 1, 1).AddHours(2).AddSeconds(seconds);
        }
    }
}
