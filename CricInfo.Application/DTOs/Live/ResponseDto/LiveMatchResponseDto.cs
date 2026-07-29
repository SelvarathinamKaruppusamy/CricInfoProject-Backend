using CricInfo.Application.DTOs.Live.ResponseDto;

public class LiveMatchResponseDto
{
    public MatchDto? Match { get; set; }

    public List<TeamDto> Teams { get; set; } = new();
}