using System;
using System.Collections.Generic;

namespace FlashScore.Football.Parsing
{
    public record H2HParserResult
    {
        public IList<Match> LastPersonalMatches { get; }
        public IList<Match> FirstTeamLastMatches { get; }
        public IList<Match> SecondTeamLastMatches { get; }
        public H2HParserResult(IList<Match> first, IList<Match> second, IList<Match> personal)
        {
            FirstTeamLastMatches = first ?? throw new ArgumentNullException(nameof(first));
            SecondTeamLastMatches = second ?? throw new ArgumentNullException(nameof(second));
            LastPersonalMatches = personal;
        }
    }
}