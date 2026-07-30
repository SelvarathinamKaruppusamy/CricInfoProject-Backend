using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Application.DTOs.Upcoming
{
    public class UpdateMatchDTO
    {
        public string? venue {  get; set; }
        //public string? city { get; set; }
        public DateOnly? date { get; set; }
    }
}
