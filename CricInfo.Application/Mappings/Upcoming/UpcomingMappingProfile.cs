using AutoMapper;
using CricInfo.Application.DTOs.Upcoming;
using CricInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace CricInfo.Application.Mappings.Upcoming
{
    public class UpcomingMappingProfile : Profile
    {
        public UpcomingMappingProfile()
        {
            CreateMap<Match, MatchDTO>()
                .ForMember(dest => dest.teams,
                    opt => opt.MapFrom(src => src.Teams));

            CreateMap<Team, TeamDTO>()
                .ForMember(dest => dest.teamId,
                    opt => opt.MapFrom(src => src.TeamId))
                .ForMember(dest => dest.players,
                    opt => opt.MapFrom(src => src.Players));

            CreateMap<Player, PlayersDTO>();
        }
    }
}
