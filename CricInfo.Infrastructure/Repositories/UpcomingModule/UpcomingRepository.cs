using CricInfo.Application.DTOs.Upcoming;
using CricInfo.Application.Interfaces.Repositories.Upcoming;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Infrastructure.Repositories.UpcomingModule
{
    public class UpcomingRepository : IUpcomingRepository
    {
        private readonly CricDbContext _context;
        public UpcomingRepository(CricDbContext context) { 
            _context = context; 
        }

        public async Task<List<Match>> GetUpcomingMatchesAsync()
        {
            return await _context.Matches
               .Include(m => m.Teams)
                   .ThenInclude(t => t.Players)
               .Where(m => m.status == "UPCOMING")
               .ToListAsync();
        }

        public async Task<Match?> GetUpcomingMatchByIdAsync(int matchNo)
        {
            return await _context.Matches
                .Include(m => m.Teams)
                    .ThenInclude(t => t.Players)
                .FirstOrDefaultAsync(m => m.matchNo == matchNo);
        }

        private string GetCityByVenue(string? venue)
{
    return venue switch
    {
        "M.A.Chidambaram Stadium" => "Chennai",
        "Wankhede Stadium" => "Mumbai",
        "Eden Gardens" => "Kolkata",
        "Narendra Modi Stadium" => "Ahmedabad",
        "Arun Jaitley Stadium" => "Delhi",
        "Ekana Cricket Stadium" => "Lucknow",
        "Rajiv Gandhi International Stadium" => "Hyderabad",
        "Sawai Mansingh Stadium" => "Jaipur",
        "M.Chinnaswamy Stadium" => "Bengaluru",
        "New PCA Stadium" => "Mohali",
        _ => string.Empty
    };
}

        public async Task<bool> UpdateUpcomingMatchAsync(int matchNo, UpdateMatchDTO dto)
        {
            var match = await _context.Matches.FirstOrDefaultAsync(m=>m.matchNo== matchNo);

            if (match == null)
            {
                return false;
            }
            match.venue= dto.venue;
            match.city = GetCityByVenue(dto.venue);
            match.date= dto.date;

            await _context.SaveChangesAsync();
            return true;    

        }
    }

}
