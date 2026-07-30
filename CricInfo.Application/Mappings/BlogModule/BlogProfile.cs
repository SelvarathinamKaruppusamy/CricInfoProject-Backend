using AutoMapper;
using CricInfo.Application.DTOs.Blog;
using CricInfo.Domain.Entities;

namespace CricInfo.Application.Mappings.BlogModule;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<Blog, BlogDto>()
            .ForMember(dest => dest.MatchId,
        opt => opt.MapFrom(src => src.MatchNo
        ))
    .ForMember(
    dest => dest.PublishedDate,
    opt => opt.MapFrom(src => src.PublishedDate.ToString("yyyy-MM-dd"))
        )
    .ForMember(
        dest => dest.Content,
        opt => opt.MapFrom(src =>
            string.IsNullOrWhiteSpace(src.Content)
                ? new List<string>()
                : src.Content.Split(
                    new[] { "\r\n\r\n", "\n\n" },
                    StringSplitOptions.RemoveEmptyEntries
                ).ToList()
        ))
    .ForMember(
        dest => dest.Tags,
        opt => opt.MapFrom(src =>
            string.IsNullOrWhiteSpace(src.Tags)
                ? new List<string>()
                : src.Tags.Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries
                )
                .Select(t => t.Trim())
                .ToList()
        ));

        CreateMap<CreateBlogDto, Blog>()
            .ForMember(
                dest => dest.MatchNo,
                opt => opt.MapFrom(src => src.MatchId)
            )
            .ForMember(dest => dest.Content, opt => opt.Ignore())
            .ForMember(dest => dest.Tags, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Content = src.Content == null
                    ? string.Empty
                    : string.Join("\r\n\r\n", src.Content);

                dest.Tags = src.Tags == null
                    ? string.Empty
                    : string.Join(",", src.Tags);
            });

        CreateMap<UpdateBlogDto, Blog>()
    .ForMember(
        dest => dest.MatchNo,
        opt => opt.MapFrom(src => src.MatchId)
    )
    .ForMember(dest => dest.Content, opt => opt.Ignore())
    .ForMember(dest => dest.Tags, opt => opt.Ignore())
    .AfterMap((src, dest) =>
    {
        dest.Content = src.Content == null
            ? string.Empty
            : string.Join("\r\n\r\n", src.Content);

        dest.Tags = src.Tags == null
            ? string.Empty
            : string.Join(",", src.Tags);
    });

        CreateMap<BlogDto, Blog>();
    }
}