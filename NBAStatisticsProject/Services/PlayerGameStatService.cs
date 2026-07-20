using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Mapping;
using NBAStatisticsProject.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace NBAStatisticsProject.Services
{
    public class PlayerGameStatService : IPlayerGameStatService
    {
        private readonly DataContext _context;
        public PlayerGameStatService(DataContext context) => _context = context;

        public async Task<List<PlayerGameStatDto>> GetAllAsync()
        {
            return await _context.PlayerGameStats
                .ToDto()
                .ToListAsync();
        }

        public async Task<PlayerGameStatDto?> GetByIdAsync(int id)
        {
            return await _context.PlayerGameStats
                .Where(pgs => pgs.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
        }
        public async Task<PlayerGameStatDto> CreateAsync(PlayerGameStatCreateDto dto)
        {
            var playerGameStat = new PlayerGameStat()
            {
                PlayerId = dto.PlayerId,
                GameId = dto.GameId,
                Points = dto.Points,
                Assists = dto.Assists,
                Rebounds = dto.Rebounds,
                MinutesPlayed = dto.MinutesPlayed,
                Steals = dto.Steals,
                Blocks = dto.Blocks,
                Turnovers = dto.Turnovers
            };
            _context.PlayerGameStats.Add(playerGameStat);
            await _context.SaveChangesAsync();
            return await _context.PlayerGameStats
                .Where(pgs => pgs.Id == playerGameStat.Id)
                .ToDto()
                .FirstAsync();
        }
        public async Task<List<PlayerGameStatDto>> CreateManyAsync(List<PlayerGameStatCreateDto> dtos)
        {
            var playerGameStats = dtos.Select(dto => new PlayerGameStat()
            {
                PlayerId = dto.PlayerId,
                GameId = dto.GameId,
                Points = dto.Points,
                Assists = dto.Assists,
                Rebounds = dto.Rebounds,
                MinutesPlayed = dto.MinutesPlayed,
                Steals = dto.Steals,
                Blocks = dto.Blocks,
                Turnovers = dto.Turnovers
            }).ToList();

            _context.PlayerGameStats.AddRange(playerGameStats);
            await _context.SaveChangesAsync();
            var ids = playerGameStats.Select(pgs => pgs.Id).ToList();

            return await _context.PlayerGameStats
                .Where(pgs => ids.Contains(pgs.Id))
                .ToDto()
                .ToListAsync();
        }
        public async Task<PlayerGameStatDto?> UpdateAsync(int id, PlayerGameStatCreateDto dto)
        {
            var existingPlayerGameStat = await _context.PlayerGameStats.FindAsync(id);
            if (existingPlayerGameStat == null) return null;

            existingPlayerGameStat.PlayerId = dto.PlayerId;
            existingPlayerGameStat.GameId = dto.GameId;
            existingPlayerGameStat.Points = dto.Points;
            existingPlayerGameStat.Assists = dto.Assists;
            existingPlayerGameStat.Rebounds = dto.Rebounds;
            existingPlayerGameStat.MinutesPlayed = dto.MinutesPlayed;
            existingPlayerGameStat.Steals = dto.Steals;
            existingPlayerGameStat.Blocks = dto.Blocks;
            existingPlayerGameStat.Turnovers = dto.Turnovers;
            await _context.SaveChangesAsync();
            return await _context.PlayerGameStats
                .Where(pgs => pgs.Id == id)
                .ToDto()
                .FirstAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existingPlayerGameStat = await _context.PlayerGameStats.FindAsync(id);
            if (existingPlayerGameStat == null) return false;
            _context.PlayerGameStats.Remove(existingPlayerGameStat);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
