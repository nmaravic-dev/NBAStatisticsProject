using NBAStatisticsProject.Models;
using System.ComponentModel.DataAnnotations;

namespace NBAStatisticsProject.DTOs
{
    public record InjuryDto(int Id, int PlayerId, DateTime StartDate, DateTime? EndDate, int Severity,string SeverityName, string Description);
    public record InjuryCreateDto([Range(1, int.MaxValue)] int PlayerId, DateTime StartDate, DateTime? EndDate, [EnumDataType(typeof(InjurySeverity))] InjurySeverity Severity, string Description);
}