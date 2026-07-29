namespace CricInfo.Domain.Entities
{ 
    public class Batting {
        public int id { get; set; }
        public int PlayerId { get; set; } 
        public int TeamId { get; set; } 
        public int MatchNo { get; set; } 
        public string? name { get; set; } 
        public string? role { get; set; } 
        public int? runs { get; set; } 
        public int? balls { get; set; } 
        public int? fours { get; set; } 
        public int? sixes { get; set; } 
        public decimal? strikeRate { get; set; } 
        public string? status { get; set; } 
    } 
}