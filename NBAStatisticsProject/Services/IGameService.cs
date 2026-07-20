using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface IGameService
    {
        Task<List<GameDto>> GetAllAsync();
        Task<GameDto?> GetByIdAsync(int id);
        Task<GameDto> CreateAsync(GameCreateDto dto);
        Task<List<GameDto>> CreateManyAsync(List<GameCreateDto> dtos);
        Task<GameDto?> UpdateAsync(int id, GameCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
