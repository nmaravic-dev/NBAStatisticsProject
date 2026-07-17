using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Mapping
{
    public static class GameMappingExtensions
    {
        public static IQueryable<GameDto> ToDto(this IQueryable<Game> query)
        {
            return query.Select(g => new GameDto(
                g.Id,
                g.Date,
                g.Season,
                g.HomeTeamId,
                g.HomeTeam!.Name,
                g.AwayTeamId,
                g.AwayTeam!.Name,
                g.HomeScore,
                g.AwayScore
            ));
        }
    }
}
