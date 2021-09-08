using MSFootball.Models.Data;
using System;
using System.Collections.Generic;

namespace MSFootball.Models.Parsing
{
    class H2HParserResult
    {
        public List<EndedMatch> LastPersonalMatches { get; }
        public List<EndedMatch> FirstTeamLastMatches { get; }
        public List<EndedMatch> SecondTeamLastMatches { get; }
        public H2HParserResult(List<EndedMatch> first, List<EndedMatch> second, List<EndedMatch> personal)
        {
            FirstTeamLastMatches = first ?? throw new ArgumentNullException(nameof(first));
            SecondTeamLastMatches = second ?? throw new ArgumentNullException(nameof(second));
            LastPersonalMatches = personal;
        }
    }
}