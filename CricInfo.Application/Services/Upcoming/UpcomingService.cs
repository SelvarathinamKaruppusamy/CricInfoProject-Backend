using AutoMapper;
using CricInfo.Application.DTOs.Upcoming;
using CricInfo.Application.Interfaces.Repositories.Upcoming;
using CricInfo.Application.Interfaces.Services.Upcoming;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace CricInfo.Application.Services.Upcoming
{
    public class UpcomingService : IUpcomingService
    {
        private readonly IUpcomingRepository _repository;
        private readonly IMapper _mapper;

        public UpcomingService(IUpcomingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MatchDTO>> GetUpcomingMatchesAsync()

        {
            var matches = await _repository.GetUpcomingMatchesAsync();
            return _mapper.Map<List<MatchDTO>>(matches);

        }

        public async Task<MatchDTO?> GetUpcomingMatchByIdAsync(int matchNo)
        {
            var match = await _repository.GetUpcomingMatchByIdAsync(matchNo);
            if(match== null)
            {
                return null;
            }

            return _mapper.Map<MatchDTO?>(match);
        }

        public async Task<bool> UpdateUpcomingMatchAsync(int matchNo, UpdateMatchDTO dto)
        {
            return await _repository.UpdateUpcomingMatchAsync(matchNo, dto);
        }
    }
}
