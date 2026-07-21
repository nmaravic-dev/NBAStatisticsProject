using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly DataContext _context;
        public WatchlistService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<WatchlistEntryDto>> GetAllAsync(string userId)
        {
            return await _context.WatchlistEntries
                .Where(w=>w.UserId == userId)
                .Select(w => new WatchlistEntryDto(
                w.Id,
                w.PlayerId,
                w.Player!.Name,
                w.Player.Position,
                w.Player.Team!.Name
                ))
                .ToListAsync();
        }

        public async Task<WatchlistEntryDto?> CreateAsync(string userId, WatchlistEntryCreateDto dto)
        {
            if (!await _context.Players.AnyAsync(p => p.Id == dto.PlayerId))
                return null;

            var alreadyExists = await _context.WatchlistEntries
                .AnyAsync(w => w.UserId == userId && w.PlayerId == dto.PlayerId);
            if (alreadyExists)
                return null;

            var entry = new WatchlistEntry
            {
                UserId = userId,           
                PlayerId = dto.PlayerId
            };
            _context.WatchlistEntries.Add(entry);
            await _context.SaveChangesAsync();

            return await _context.WatchlistEntries
                .Where(w => w.Id == entry.Id)
                .Select(w => new WatchlistEntryDto(
                    w.Id, w.PlayerId, w.Player!.Name, w.Player.Position, w.Player.Team!.Name))
                .FirstAsync();
        }

        public async Task<bool> DeleteAsync(string userId, int id)
        {
            var entry = await _context.WatchlistEntries
                .Where(w => w.Id == id && w.UserId == userId)
                .FirstOrDefaultAsync();
            if (entry == null)
                return false;

            _context.WatchlistEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
