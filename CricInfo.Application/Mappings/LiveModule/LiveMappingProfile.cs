using AutoMapper;
using CricInfo.Application.DTOs.Live.RequestDto;
using CricInfo.Application.DTOs.Live.ResponseDto;
using CricInfo.Domain.Entities;

public class LiveMappingProfile : Profile
{
    public LiveMappingProfile()
    {
        //get dto
        CreateMap<Match, MatchDto>();

        CreateMap<Team, TeamDto>();

        CreateMap<Player, PlayerDto>();
        //update dto
        CreateMap<MatchUpdateDto, Match>();
        CreateMap<TeamUpdateDto, Team>();
        CreateMap<PlayerUpdateDto, Player>();
    }
}