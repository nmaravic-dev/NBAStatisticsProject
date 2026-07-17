using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Mapping
{
    public static class PlayerGameStatMappingExtensions
    {
        public static IQueryable<PlayerGameStatDto> ToDto(this IQueryable<PlayerGameStat> query)
        {
            return query.Select(pgs => new PlayerGameStatDto(
                pgs.Id,
                pgs.PlayerId,
                pgs.Player!.Name,
                pgs.GameId,
                pgs.Game!.Date,
                pgs.Game.HomeTeam!.Name,
                pgs.Game.AwayTeam!.Name,
                pgs.Points,
                pgs.Assists,
                pgs.Rebounds,
                pgs.MinutesPlayed,
                pgs.Steals,
                pgs.Blocks,
                pgs.Turnovers
            ));
        }
    }
}
