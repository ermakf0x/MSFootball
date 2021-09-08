using MSFootball.Models.Data;
using System.Linq;

namespace MSFootball.Models.Analiz
{
    /// <summary>
    /// Фильтер. Ничья
    /// </summary>
    class FDraw : FilterBase
    {
        public override FilterProperties FilterProperties { get; protected set; }

        #region Filter Property

        /// <summary>
        /// Тайм за который смотрим статистику
        /// </summary>
        [FilterProperty("Тайм")]
        ComboBoxEnumFilterProperty<Half> half;

        /// <summary>
        /// % значение
        /// </summary>
        [FilterProperty("% (1-99)")]
        SliderFilterProperty sliderValue;

        #endregion

        public FDraw()
        {
            FilterProperties = new FilterProperties(this);

            half = new ComboBoxEnumFilterProperty<Half>();
            sliderValue = new SliderFilterProperty(1, 100, 1);
        }

        public override FilterResult ApplyFilter(Team team)
        {
            if (team == null || team.LastMatches == null || team.LastMatches.Count == 0) return null;

            var _half = half.Value;
            var _count = team.LastMatches.Where(match =>
            {
                var teamName = match.GetResult(_half);
                if (teamName == null) return false;

                return teamName == "";
            }).Count();


            return IsFit(_count, team.LastMatches.Count, sliderValue) ? new FilterResult(team) : null;
        }
    }
}
