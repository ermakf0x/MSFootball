using MSFootball.Models.Data;

namespace MSFootball.Models.Analiz
{
    /// <summary>
    /// Базовый абстрактный класс фильтра
    /// </summary>
    abstract class FilterBase
    {
        /// <summary>
        /// Включен фильтер или нет
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Имя фильтра
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Свойства для настройки фильтра
        /// </summary>
        public abstract FilterProperties FilterProperties { get; protected set; }

        public FilterBase()
        {
            Enabled = true;
            FilterProperties.Build();
        }

        /// <summary>
        /// Применяет фильтер к команде
        /// </summary>
        /// <param name="team">Футбольная команда</param>
        /// <returns>Результат после приминения фильтра</returns>
        public abstract FilterResult ApplyFilter(Team team);

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? base.ToString() : Name;
        }

        protected bool IsFit(int count, int allCount, SliderFilterProperty slider)
        {
            var a = (double)count / allCount * 100; // получаем процент подходящих матчей из всех сыгранных матчей
            var a2 = slider.Value;
            return a >= a2;
        }
    }
}
