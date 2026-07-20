using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface IPlayerGameStatService
    {
        Task<List<PlayerGameStatDto>> GetAllAsync();
        Task<PlayerGameStatDto?> GetByIdAsync(int id);
        Task<PlayerGameStatDto> CreateAsync(PlayerGameStatCreateDto dto);
        Task<List<PlayerGameStatDto>> CreateManyAsync(List<PlayerGameStatCreateDto> dtos);
        Task<PlayerGameStatDto?> UpdateAsync(int id, PlayerGameStatCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
