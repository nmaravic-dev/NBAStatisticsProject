using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Mapping;
using NBAStatisticsProject.Models;


namespace NBAStatisticsProject.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly DataContext _context;
        public PlayerService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<PlayerDto>> GetAllAsync()
        {
            return await _context.Players
                .ToDto()
                .ToListAsync();
        }
        public async Task<PlayerDto?> GetByIdAsync(int id)
        {
            return await _context.Players
                .Where(p => p.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
        }
        public async Task<List<PlayerDto>> GetPlayersByTeamAsync(int teamId) {
            return await _context.Players
                .Where(p => p.TeamId == teamId)
                .ToDto()
                .ToListAsync();
        }
        public async Task<int> GetCountAsync() => await _context.Players.CountAsync();

        public async Task<PlayerDto?> CreateAsync(PlayerCreateDto dto)
        {
            if (!await _context.Teams.AnyAsync(t => t.Id == dto.TeamId))
                return null;

            var player = new Player
            {
                Name = dto.Name,
                Position = dto.Position,
                TeamId = dto.TeamId
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            var createdPlayer = await _context.Players
                .Where(p => p.Id == player.Id)
                .ToDto()
                .FirstAsync();
            return createdPlayer;
        }
        public async Task<List<PlayerDto>> CreateManyAsync(List<PlayerCreateDto> dtos)
        {
            var players = dtos.Select(dto => new Player
            {
                Name = dto.Name,
                Position = dto.Position,
                TeamId = dto.TeamId
            }).ToList();
            _context.Players.AddRange(players);
            await _context.SaveChangesAsync();
            var ids = players.Select(p => p.Id).ToList();

            return await _context.Players
                .Where(p => ids.Contains(p.Id))
                .ToDto()
                .ToListAsync();
        }
        public async Task<PlayerDto?> UpdateAsync(int id, PlayerCreateDto dto)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return null;
            player.Name = dto.Name;
            player.Position = dto.Position;
            player.TeamId = dto.TeamId;
            await _context.SaveChangesAsync();
            return await _context.Players
                .Where(p => p.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return false;
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
