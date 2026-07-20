using NBAStatisticsProject.Models;
using System.ComponentModel.DataAnnotations;

namespace NBAStatisticsProject.DTOs
{
    public record InjuryDto(int Id, int PlayerId, DateTime StartDate, DateTime? EndDate, InjurySeverity Severity, string Description);
    public record InjuryCreateDto([Range(1, int.MaxValue)] int PlayerId, DateTime StartDate, DateTime? EndDate, InjurySeverity Severity, string Description);
}