using MSFootball.Models.Data;
using System.Linq;

namespace MSFootball.Models.Analiz
{
    /// <summary>
    /// Фильтер. Тотал забитых и пропущенных
    /// </summary>
    class FTotal : FilterBase
    {
        public override FilterProperties FilterProperties { get; protected set; }

        #region Filter Property
        
        /// <summary>
        /// Тайм за который смотрим статистику
        /// </summary>
        [FilterProperty("Тайм")]
        protected ComboBoxEnumFilterProperty<Half> half;

        /// <summary>
        /// Условие. Больше или меньше
        /// </summary>
        [FilterProperty("Условие")]
        protected ComboBoxEnumFilterProperty<LessMoreEqual> smallerOrMore;

        /// <summary>
        /// Условие. Во сколько раз больше или меньше
        /// </summary>
        [FilterProperty("Условие")]
        protected SliderFilterProperty sliderValue;

        /// <summary>
        /// % значение
        /// </summary>
        [FilterProperty("% (1-99)")]
        protected SliderFilterProperty sliderValue2;

        #endregion

        public FTotal()
        {
            FilterProperties = new FilterProperties(this);

            half = new ComboBoxEnumFilterProperty<Half>();
            smallerOrMore = new ComboBoxEnumFilterProperty<LessMoreEqual>();
            sliderValue = new SliderFilterProperty(0.5, 5, 0.5);
            sliderValue2 = new SliderFilterProperty(1, 100, 1);
        }

        public override FilterResult ApplyFilter(Team team)
        {
            if (team == null || team.LastMatches == null || team.LastMatches.Count == 0) return null;

            var _half = half.Value;
            var _sm = smallerOrMore.Value;
            var _value = sliderValue.Value;
            var _count = 0;

            if (_sm == LessMoreEqual.More)
            {
                _count = team.LastMatches.Where(match =>
                {
                    if (match.Score == null) return false;
                    var _score = match.Score[_half];
                    if (_score == null) return false;
                    var sum = _score.Value.First + _score.Value.Second;
                    return sum > _value;
                }).Count();
            }
            else
            {
                _count = team.LastMatches.Where(match =>
                {
                    if (match.Score == null) return false;
                    var _score = match.Score[_half];
                    if (_score == null) return false;
                    var sum = _score.Value.First + _score.Value.Second;
                    return sum < _value;
                }).Count();
            }

            return IsFit(_count, team.LastMatches.Count, sliderValue2) ? new FilterResult(team) : null;
        }
    }
}
