using NBAStatisticsProject.Models;
using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Mapping
{
    public static class InjuryMappingExtensions
    {
        public static IQueryable<InjuryDto> ToDto(this IQueryable<Injury> query)
        {
            return query.Select(i => new InjuryDto(
                i.Id,
                i.PlayerId,
                i.StartDate,
                i.EndDate,
                i.Severity,
                i.Description
            ));
        }   
    }
}
