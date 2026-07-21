using AutoMapper;
using CricInfo.Application.DTOs.CompletedModule;
using CricInfo.Domain.Entities;

namespace CricInfo.Application.Mapping.CompletedModule;

public class CompletedMappingProfile : Profile
{
    public CompletedMappingProfile()
    {
        CreateMap<Match, CompletedMatchResponseDto>();

        CreateMap<Team, CompletedTeamResponseDto>();

        CreateMap<Batting, BattingResponseDto>();

        CreateMap<Bowling, BowlingResponseDto>();
    }
}