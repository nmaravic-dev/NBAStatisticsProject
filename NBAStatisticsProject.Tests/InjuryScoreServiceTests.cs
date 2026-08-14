using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.Models;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Tests
{
    public class InjuryScoreServiceTests : IDisposable
    {
        private readonly DataContext _context;

        private readonly InjuryScoreService _service;

        public InjuryScoreServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DataContext(options);
            _service = new InjuryScoreService(_context, NullLogger<InjuryScoreService>.Instance);
        }
        public void Dispose() => _context.Dispose();

        [Fact]
        public async Task GetInjuryScore_PlayerWithNoInjuries_Returns10()
        {
            _context.Players.Add(new Player { Id = 1, FirstName = "Test", LastName = "Player", Position = "PG", TeamId = 1 });
            await _context.SaveChangesAsync();

            var result = await _service.GetInjuryScoreAsync(1);

            Assert.NotNull(result);
            Assert.Equal(10.0, result.Score);
        }

        [Fact]
        public async Task GetInjuryScore_NonExistentPlayer_ReturnsNull()
        {

            var result = await _service.GetInjuryScoreAsync(999);  

            Assert.Null(result);
        }

        [Fact]
        public async Task GetInjuryScore_PlayerWithInjury_ScoreBelowTen()
        {
            _context.Players.Add(new Player { Id = 1, FirstName = "Injured", LastName = "Player", Position = "C", TeamId = 1 });

            _context.Injuries.Add(new Injury
            {
                Id = 1,
                PlayerId = 1,
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 1, 31),    
                Severity = InjurySeverity.Severe,
                Description = "Test injury"
            });

            _context.Games.Add(new Game 
            { 
                Id = 1, 
                Date = new DateTime(2026, 1, 10), 
                Season = "2025-26", 
                HomeTeamId = 1, 
                AwayTeamId = 2, 
                HomeScore = 100, 
                AwayScore = 95 
            });
            _context.Games.Add(new Game 
            { 
                Id = 2, 
                Date = new DateTime(2026, 1, 20), 
                Season = "2025-26", HomeTeamId = 1, 
                AwayTeamId = 3, 
                HomeScore = 110, 
                AwayScore = 108 
            });

            _context.Games.Add(new Game 
            { 
                Id = 3, 
                Date = new DateTime(2026, 2, 5), 
                Season = "2025-26", 
                HomeTeamId = 1, 
                AwayTeamId = 2, 
                HomeScore = 105, 
                AwayScore = 100 
            });

            _context.PlayerGameStats.Add(new PlayerGameStat 
            { 
                Id = 1, 
                PlayerId = 1, 
                GameId = 3,
                Points = 20,
                Rebounds = 10,
                Assists = 5, 
                MinutesPlayed = 30, 
                Steals = 1, 
                Blocks = 1, 
                Turnovers = 2 
            });

            await _context.SaveChangesAsync();

            var result = await _service.GetInjuryScoreAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Score < 10.0);
        }

        [Theory]
        [InlineData(InjurySeverity.Minor)]
        [InlineData(InjurySeverity.Moderate)]
        [InlineData(InjurySeverity.Severe)]
        public async Task GetInjuryScore_AnySeverity_ScoreInValidRange(InjurySeverity severity)
        {
            _context.Players.Add(new Player { Id = 1, FirstName = "Test", LastName = "Player", Position = "PG", TeamId = 1 });

            _context.Injuries.Add(new Injury
            {
                PlayerId = 1,
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 1, 31),
                Severity = severity,
                Description = "Test injury"
            });

            _context.Games.Add(new Game { Id = 1, Date = new DateTime(2026, 1, 10), Season = "2025-26", HomeTeamId = 1, AwayTeamId = 2, HomeScore = 100, AwayScore = 95 });
            _context.Games.Add(new Game { Id = 2, Date = new DateTime(2026, 2, 5), Season = "2025-26", HomeTeamId = 1, AwayTeamId = 3, HomeScore = 105, AwayScore = 100 });
            _context.PlayerGameStats.Add(new PlayerGameStat { Id = 1, PlayerId = 1, GameId = 2, Points = 20, Rebounds = 10, Assists = 5, MinutesPlayed = 30, Steals = 1, Blocks = 1, Turnovers = 2 });

            await _context.SaveChangesAsync();

            var result = await _service.GetInjuryScoreAsync(1);

            Assert.NotNull(result);
            Assert.InRange(result.Score, 0.0, 10.0);
        }

        [Fact]
        public async Task GetAllInjuryScores_HealthyAndInjuredPlayer_ReturnsScoreForBoth()
        {
            _context.Players.Add(new Player { Id = 1, FirstName = "Injured", LastName = "Player", Position = "C", TeamId = 1 });

            _context.Players.Add(new Player { Id = 2, FirstName = "Healthy", LastName = "Player", Position = "PG", TeamId = 1 });

            _context.Injuries.Add(new Injury
            {
                Id = 1,
                PlayerId = 1,
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 1, 31),
                Severity = InjurySeverity.Severe,
                Description = "Test injury"
            });

            _context.Games.Add(new Game
            {
                Id = 1,
                Date = new DateTime(2026, 1, 10),
                Season = "2025-26",
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeScore = 100,
                AwayScore = 95
            });

            _context.Games.Add(new Game
            {
                Id = 2,
                Date = new DateTime(2026, 2, 5),
                Season = "2025-26",
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeScore = 105,
                AwayScore = 100
            });

            _context.PlayerGameStats.Add(new PlayerGameStat
            {
                Id = 1,
                PlayerId = 1,
                GameId = 2,
                Points = 20,
                Rebounds = 10,
                Assists = 5,
                MinutesPlayed = 30,
                Steals = 1,
                Blocks = 1,
                Turnovers = 2
            });

            await _context.SaveChangesAsync();

            var result = await _service.GetAllInjuryScoresAsync();

            Assert.Equal(2, result.Count);

            var injured = result.Single(s => s.PlayerId == 1);
            var healthy = result.Single(s => s.PlayerId == 2);

            Assert.True(injured.Score < 10.0);
            Assert.Equal(10.0, healthy.Score);
        }

        [Fact]
        public async Task GetAllInjuryScores_NoPlayers_ReturnsEmptyList()
        {
            var result = await _service.GetAllInjuryScoresAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
