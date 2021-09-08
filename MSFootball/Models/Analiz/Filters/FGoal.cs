using MSFootball.Models.Data;
using System.Linq;

namespace MSFootball.Models.Analiz
{
    class FGoal : FilterBase
    {
        public override FilterProperties FilterProperties { get; protected set; }

        #region Filter Property

        /// <summary>
        /// Промежуток времени в который был гол
        /// </summary>
        [FilterProperty("Время гола")]
        RangeSliderFilterProperty time;

        /// <summary>
        /// % значение
        /// </summary>
        [FilterProperty("% (1-99)")]
        protected SliderFilterProperty sliderValue;

        #endregion

        public FGoal()
        {
            FilterProperties = new FilterProperties(this);
            time = new RangeSliderFilterProperty(1, 90, 1);
            sliderValue = new SliderFilterProperty(1, 100, 1);
        }

        public override FilterResult ApplyFilter(Team team)
        {
            if (team == null || team.LastMatches == null || team.LastMatches.Count == 0) return null;

            var _time1 = time.Value;
            var _time2 = time.Value2;

            var _count = team.LastMatches.Where(m =>
            {
                if (m.Summary is null || m.Summary.IsEmpty) return false;

                
                var summary = m.Summary[Half.Match];

                foreach (var e in summary.EventList)
                {
                    if (e.Type != MatchEventType.Goal) continue;
                    var time = (int)e.Time;
                    if (_time1 <= time && time <= _time2) return true;
                }

                return false;
            }).Count();

            return IsFit(_count, team.LastMatches.Count, sliderValue) ? new FilterResult(team) : null;
        }
    }
}
