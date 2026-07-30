using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.Models;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Tests
{
    public class InjuryScoreServiceTests
    {
        [Fact]
        public async Task GetInjuryScore_PlayerWithNoInjuries_Returns10()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new DataContext(options);

            context.Players.Add(new Player { Id = 1, Name = "Test Player", Position = "PG", TeamId = 1 });
            await context.SaveChangesAsync();

            var service = new InjuryScoreService(context);

            var result = await service.GetInjuryScoreAsync(1);

            Assert.NotNull(result);
            Assert.Equal(10.0, result.Score);
        }
        [Fact]
        public async Task GetInjuryScore_NoexistentPlayer_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new DataContext(options);

            var service = new InjuryScoreService(context);

            var result = await service.GetInjuryScoreAsync(999);  

            Assert.Null(result);
        }

        [Fact]
        public async Task GetInjuryScore_PlayerWithInjury_ScoreBelowTen()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var context = new DataContext(options);

            context.Players.Add(new Player { Id = 1, Name = "Injured", Position = "C", TeamId = 1 });

            context.Injuries.Add(new Injury
            {
                Id = 1,
                PlayerId = 1,
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 1, 31),    
                Severity = InjurySeverity.Severe,
                Description = "Test injury"
            });

            context.Games.Add(new Game 
            { 
                Id = 1, 
                Date = new DateTime(2026, 1, 10), 
                Season = "2025-26", 
                HomeTeamId = 1, 
                AwayTeamId = 2, 
                HomeScore = 100, 
                AwayScore = 95 
            });
            context.Games.Add(new Game 
            { 
                Id = 2, 
                Date = new DateTime(2026, 1, 20), 
                Season = "2025-26", HomeTeamId = 1, 
                AwayTeamId = 3, 
                HomeScore = 110, 
                AwayScore = 108 
            });

            context.Games.Add(new Game 
            { 
                Id = 3, 
                Date = new DateTime(2026, 2, 5), 
                Season = "2025-26", 
                HomeTeamId = 1, 
                AwayTeamId = 2, 
                HomeScore = 105, 
                AwayScore = 100 
            });

            context.PlayerGameStats.Add(new PlayerGameStat 
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

            await context.SaveChangesAsync();

            var service = new InjuryScoreService(context);

            var result = await service.GetInjuryScoreAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Score < 10.0);
        }
    }
}
