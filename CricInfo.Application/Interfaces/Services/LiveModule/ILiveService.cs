using CricInfo.Application.DTOs.Live;

namespace CricInfo.Application.Interfaces.Services.LiveModule;

public interface ILiveService
{
    Task<LiveMatchResponseDto?> GetLiveMatchAsync(int matchNo);
}