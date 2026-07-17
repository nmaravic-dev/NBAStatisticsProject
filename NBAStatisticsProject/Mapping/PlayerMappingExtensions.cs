using NBAStatisticsProject.Models;
using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Mapping
{
    public static class PlayerMappingExtensions
    {
        public static IQueryable<PlayerDto> ToDto(this IQueryable<Player> query)
        {
            return query.Select(p => new PlayerDto(
                p.Id,
                p.Name,
                p.Position,
                p.TeamId,
                p.Team!.Name
            ));
        }
    }
}
