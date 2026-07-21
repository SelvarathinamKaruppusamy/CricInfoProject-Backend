using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Domain.Entities
{
    public class Match
    {
        public int id { get; set; }

        public int matchNo { get; set; }

        public string? venue { get; set; }

        public string? city { get; set; }

        public DateOnly? date { get; set; }

        public string? tossWinner { get; set; }

        public string? tossDecision { get; set; }

        public string? result { get; set; }

        public string? playerOfTheMatch { get; set; }

        public string? status { get; set; }

        public int? currentInnings { get; set; }

        public int? currentBattingTeamIndex { get; set; }

        public int? currentBowlingTeamIndex { get; set; }

        public int? strikerPlayerId { get; set; }

        public int? nonStrikerPlayerId { get; set; }

        public int? currentBowlerPlayerId { get; set; }

        // Navigation Property
        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
