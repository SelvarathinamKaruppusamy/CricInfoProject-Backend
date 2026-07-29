using CricInfo.Application.DTOs.Upcoming;
using CricInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Application.Interfaces.Repositories.Upcoming
{
    public interface IUpcomingRepository
    {
        Task<List<Match>> GetUpcomingMatchesAsync();
        Task <Match?> GetUpcomingMatchByIdAsync(int matchNo);
        Task<bool> UpdateUpcomingMatchAsync(int matchNo, UpdateMatchDTO dto);
    }
}
