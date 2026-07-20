using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Domain.Entities
{
    public class Player
    {
        public int id { get; set; }

        public int playerId { get; set; }

        public int TeamId { get; set; }

        public int matchNo { get; set; }

        public string? name { get; set; }

        public string? role { get; set; }

        public int? runs { get; set; }

        public int? balls { get; set; }

        public int? fours { get; set; }

        public int? sixes { get; set; }

        public decimal? strikeRate { get; set; }

        public string? status { get; set; }

        public decimal? overs { get; set; }

        public int? wickets { get; set; }

        public int? maidens { get; set; }

        public int? runsConceded { get; set; }

        public decimal? economy { get; set; }

        // Navigation Property
        public Team Team { get; set; } = null!;
    }
}
