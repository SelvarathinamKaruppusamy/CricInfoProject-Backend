using AutoMapper;
using CricInfo.Application.DTOs.Live;
using CricInfo.Domain.Entities;

public class LiveMappingProfile : Profile
{
    public LiveMappingProfile()
    {
        CreateMap<Match, MatchDto>();

        CreateMap<Team, TeamDto>();

        CreateMap<Player, PlayerDto>();
    }
}