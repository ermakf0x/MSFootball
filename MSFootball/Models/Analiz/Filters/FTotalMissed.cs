using MSFootball.Models.Data;
using System.Linq;

namespace MSFootball.Models.Analiz
{
    /// <summary>
    /// Фильтер. Тотал пропущенных мячей.
    /// </summary>
    class FTotalMissed : FTotal
    {
        public override FilterResult ApplyFilter(Team team)
        {
            if (team == null || team.LastMatches == null || team.LastMatches.Count == 0) return null;

            var _half = half.Value;
            var _sm = smallerOrMore.Value;
            var _value = sliderValue.Value;
            var _teamName = team.Name;
            var _count = 0;

            if (_sm == LessMoreEqual.More)
            {
                _count = team.LastMatches.Where(match =>
                {
                    if (match.Score == null) return false;
                    var _score = match.Score[_half];
                    if (_score == null) return false;
                    var a = _teamName == match.TeamName2 ? _score.Value.First : _score.Value.Second;
                    return a > _value;
                }).Count();
            }
            else
            {
                _count = team.LastMatches.Where(match =>
                {
                    if (match.Score == null) return false;
                    var _score = match.Score[_half];
                    if (_score == null) return false;
                    var a = _teamName == match.TeamName2 ? _score.Value.First : _score.Value.Second;
                    return a < _value;
                }).Count();
            }

            return IsFit(_count, team.LastMatches.Count, sliderValue2) ? new FilterResult(team) : null;
        }
    }
}
