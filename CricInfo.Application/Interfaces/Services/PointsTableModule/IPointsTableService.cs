using CricInfo.Application.DTOs.PointsTable;

namespace CricInfo.Application.Interfaces.Services.PointsTableModule;

public interface IPointsTableService
{
    Task<List<PointsTableDto>> GetLatestPointsTableAsync();

    Task<List<PointsTableDto>> GetPointsTableAsync(int matchNo);

}