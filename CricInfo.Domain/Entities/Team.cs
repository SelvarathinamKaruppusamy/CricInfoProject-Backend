using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Domain.Entities
{
    public class Team
    {
        public int id { get; set; }

        public int TeamId { get; set; }

        public int matchNo { get; set; }

        public string? fullName { get; set; }

        public string? shortName { get; set; }

        public string? logo { get; set; }

        public string? scores { get; set; }

        public int? runs { get; set; }

        public int? wickets { get; set; }

        public int? extras { get; set; }

        public decimal? overs { get; set; }

        public int? balls { get; set; }

        public int? winCount { get; set; }

        public int? lossCount { get; set; }

        public int? totalMatch { get; set; }

        public string? matchStatus { get; set; }

        // Navigation Properties
        public Match Match { get; set; } = null!;

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
