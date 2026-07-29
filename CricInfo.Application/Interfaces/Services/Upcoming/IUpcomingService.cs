using CricInfo.Application.DTOs.Upcoming;
using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Application.Interfaces.Services.Upcoming
{
    public interface IUpcomingService
    {
        Task<List<MatchDTO>> GetUpcomingMatchesAsync();
        Task<MatchDTO?> GetUpcomingMatchByIdAsync(int matchNo);

        Task<bool> UpdateUpcomingMatchAsync(int matchNo, UpdateMatchDTO dto);


    }
}
