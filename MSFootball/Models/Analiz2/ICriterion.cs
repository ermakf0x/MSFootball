using MSFootball.Models.Data;
using System.Runtime.CompilerServices;

namespace MSFootball.Models.Analiz2
{
    public interface ICriterion
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool CanFit(EndedMatch match);
    }
}
