using CricInfo.Application.DTOs.Live;
using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Application.DTOs.Upcoming
{
    public class TeamDTO
    {
        public int teamId { get; set; }
        public string fullName { get; set; }=string.Empty;
        public string shortName {  get; set; }=string.Empty;

        public string logo {  get; set; }=string.Empty;
        public int winCount { get; set; }
        public int lossCount { get; set; }
        public string? matchStatus { get; set; }

        public List<PlayersDTO> players { get; set; } = new();
    }
}
