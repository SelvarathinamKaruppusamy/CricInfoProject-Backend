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

        CreateMap<Batting, BattingResponseDto>()
    .ForMember(dest => dest.playerId,
        opt => opt.MapFrom(src => src.PlayerId));

        CreateMap<Bowling, BowlingResponseDto>()
            .ForMember(dest => dest.playerId,
                opt => opt.MapFrom(src => src.PlayerId));

        CreateMap<Player, Batting>()
            .ForMember(dest => dest.id, opt => opt.Ignore());

        CreateMap<Player, Bowling>()
            .ForMember(dest => dest.id, opt => opt.Ignore());
    }
    
}