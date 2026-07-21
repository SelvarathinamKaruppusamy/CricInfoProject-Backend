using CricInfo.Application.DTOs.CompletedModule;

namespace CricInfo.Application.Interfaces.Services.CompletedModule;

public interface ICompletedService
{
    Task<CompletedMatchResponseDto?> GetCompletedMatchAsync(int matchNo);
}