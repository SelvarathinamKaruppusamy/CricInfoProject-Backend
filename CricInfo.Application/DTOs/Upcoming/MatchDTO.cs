using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Application.DTOs.Upcoming
{
    public class MatchDTO
    {
        public int matchNo {  get; set; }
        public string venue { get; set; }= string.Empty;
        public string city {  get; set; }= string.Empty;
        public DateOnly? date { get; set; }
        public string status {  get; set; }= string.Empty;
        public List<TeamDTO> teams { get; set; } = new();
    }
}
