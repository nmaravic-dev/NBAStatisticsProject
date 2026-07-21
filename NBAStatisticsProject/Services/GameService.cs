using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Mapping;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Services
{
    public class GameService(DataContext context) : IGameService
    {
        private readonly DataContext _context = context;

        public async Task<List<GameDto>> GetAllAsync()
        {
            return await _context.Games
                .ToDto()
                .ToListAsync();
        }
        public async Task<GameDto?> GetByIdAsync(int id)
        {
            return await _context.Games
                .Where(g => g.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
        }

        public async Task<GameDto?> CreateAsync(GameCreateDto dto)
        {
            if (dto.Date > DateTime.UtcNow)
                return null;
            if (dto.HomeTeamId == dto.AwayTeamId)
                return null;
            var teamIds = new[] { dto.HomeTeamId, dto.AwayTeamId };
            var existingCount = await _context.Teams
                .CountAsync(t => teamIds.Contains(t.Id));
            if (existingCount != 2)
                return null;

            var game = new Game
            {
                Date = dto.Date,
                Season = dto.Season,
                HomeTeamId = dto.HomeTeamId,
                AwayTeamId = dto.AwayTeamId,
                HomeScore = dto.HomeScore,
                AwayScore = dto.AwayScore
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return await _context.Games
                .Where(g => g.Id == game.Id)
                .ToDto()
                .FirstAsync();
        }

        public async Task<List<GameDto>> CreateManyAsync(List<GameCreateDto> dtos)
        {
            var games = dtos.Select(dto => new Game
            {
                Date = dto.Date,
                Season = dto.Season,
                HomeTeamId = dto.HomeTeamId,
                AwayTeamId = dto.AwayTeamId,
                HomeScore = dto.HomeScore,
                AwayScore = dto.AwayScore
            }).ToList();
            _context.Games.AddRange(games);
            await _context.SaveChangesAsync();
            var ids = games.Select(g => g.Id).ToList();
            return await _context.Games
                .Where(g => ids.Contains(g.Id))
                .ToDto()
                .ToListAsync();
        }

        public async Task<GameDto?> UpdateAsync(int id, GameCreateDto dto)
        {
            if (dto.Date > DateTime.UtcNow)
                return null;
            if (dto.HomeTeamId == dto.AwayTeamId)
                return null;

            var teamIds = new[] { dto.HomeTeamId, dto.AwayTeamId };
            var existingCount = await _context.Teams
                .CountAsync(t => teamIds.Contains(t.Id));
            if (existingCount != 2)
                return null;

            var game = await _context.Games.FindAsync(id);
            if (game == null) return null;
            game.Date = dto.Date;
            game.Season = dto.Season;
            game.HomeTeamId = dto.HomeTeamId;
            game.AwayTeamId = dto.AwayTeamId;
            game.HomeScore = dto.HomeScore;
            game.AwayScore = dto.AwayScore;
            await _context.SaveChangesAsync();
            return await _context.Games
                .Where(g => g.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return false;
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
