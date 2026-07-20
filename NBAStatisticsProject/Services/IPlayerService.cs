using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface IPlayerService
    {
        Task<List<PlayerDto>> GetAllAsync();
        Task<PlayerDto?> GetByIdAsync(int id);
        Task<List<PlayerDto>> GetPlayersByTeamAsync(int teamId);
        Task<int> GetCountAsync();
        Task<PlayerDto?> CreateAsync(PlayerCreateDto dto);
        Task<List<PlayerDto>> CreateManyAsync(List<PlayerCreateDto> dtos);
        Task<PlayerDto?> UpdateAsync(int id, PlayerCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
