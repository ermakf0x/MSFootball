using MSFootball.Models.Data;
using System.Linq;

namespace MSFootball.Models.Analiz
{
    class FStatistics : FilterBase
    {
        public override FilterProperties FilterProperties { get; protected set; }

        #region Filter Property

        /// <summary>
        /// Тайм за который смотрим статистику
        /// </summary>
        [FilterProperty("Тайм")]
        ComboBoxEnumFilterProperty<Half> half;

        /// <summary>
        /// Вид статистики
        /// </summary>
        [FilterProperty("Вид статистики")]
        ComboBoxEnumFilterProperty<StatisticType> statistic;

        /// <summary>
        /// Условие. Больше или меньше
        /// </summary>
        [FilterProperty("Условие")]
        ComboBoxEnumFilterProperty<LessMoreEqual> smallerOrMore;

        /// <summary>
        /// Условие. Во сколько раз больше или меньше
        /// </summary>
        [FilterProperty("Условие")]
        SliderFilterProperty sliderValue;

        /// <summary>
        /// % значение
        /// </summary>
        [FilterProperty("% (1-99)")]
        SliderFilterProperty sliderValue2;

        #endregion

        public FStatistics()
        {
            FilterProperties = new FilterProperties(this);
            half = new ComboBoxEnumFilterProperty<Half>();
            statistic = new ComboBoxEnumFilterProperty<StatisticType>();
            smallerOrMore = new ComboBoxEnumFilterProperty<LessMoreEqual>();
            sliderValue = new SliderFilterProperty(0, 1000, 1);
            sliderValue2 = new SliderFilterProperty(1, 100, 1);
        }

        public override FilterResult ApplyFilter(Team team)
        {
            if (team == null || team.LastMatches == null || team.LastMatches.Count == 0) return null;

            var _half = half.Value;
            var _statistic = statistic.Value;
            var _sm = smallerOrMore.Value;
            var _value = sliderValue.Value;
            var _count = 0;

            if (_sm == LessMoreEqual.More)
            {
                _count = team.LastMatches.Where(match =>
                {
                    if (match.Statistic is null || match.Statistic.IsEmpty) return false;

                    var _statHalf = match.Statistic[_half];
                    var _stat = _statHalf[_statistic];
                    if (_stat == null) return false;
                    var _value2 = match.TeamName1 == team.Name ? _stat.Value.Value : _stat.Value.Value2;
                    return _value2 > _value;
                }).Count();
            }
            else
            {
                _count = team.LastMatches.Where(match =>
                {
                    if (match.Statistic is null || match.Statistic.IsEmpty) return false;

                    var _statHalf = match.Statistic[_half];
                    var _stat = _statHalf[_statistic];
                    if (_stat == null) return false;
                    var _value2 = match.TeamName1 == team.Name ? _stat.Value.Value : _stat.Value.Value2;
                    return _value2 < _value;
                }).Count();
            }

            return IsFit(_count, team.LastMatches.Count, sliderValue2) ? new FilterResult(team) : null;
        }
    }
}
