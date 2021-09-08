using MSFootball.Models.Data;
using System.Linq;

namespace MSFootball.Models.Analiz
{
    /// <summary>
    /// Фильтер. Оба забьют.
    /// </summary>
    class FBothWillScore : FilterBase
    {
        public override FilterProperties FilterProperties { get; protected set; }

        #region Filter Property

        /// <summary>
        /// Тайм за который смотрим статистику
        /// </summary>
        [FilterProperty("Тайм")]
        ComboBoxEnumFilterProperty<Half> half;

        /// <summary>
        /// Да/Нет
        /// </summary>
        [FilterProperty("Да/Нет")]
        ComboBoxTextFilterProperty yesNo;

        [FilterProperty("% (1-99)")]
        SliderFilterProperty sliderValue;

        #endregion

        public FBothWillScore()
        {
            FilterProperties = new FilterProperties(this);

            half = new ComboBoxEnumFilterProperty<Half>();
            yesNo = new ComboBoxTextFilterProperty("Да", "Нет");
            sliderValue = new SliderFilterProperty(1, 100, 1);
        }

        public override FilterResult ApplyFilter(Team team)
        {
            if (team == null || team.LastMatches == null || team.LastMatches.Count == 0) return null;

            var _half = half.Value;
            var _count = 0;

            if (yesNo.Value == "Да")
            {
                _count = team.LastMatches.Where(match =>
                {
                    var _score = match.Score[_half];
                    if (_score == null) return false;
                    return _score.Value.First > 0 && _score.Value.Second > 0;
                }).Count();
            }
            else
            {
                _count = team.LastMatches.Where(match =>
                {
                    var _score = match.Score[_half];
                    if (_score == null) return false;
                    return _score.Value.First > 0 && _score.Value.Second > 0;
                }).Count();
            }

            return IsFit(_count, team.LastMatches.Count, sliderValue) ? new FilterResult(team) : null;
        }
    }
}
