namespace NBAStatisticsProject.Models
{

    public class Injury
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player? Player { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public InjurySeverity Severity { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public enum InjurySeverity
    {
        Minor = 1,
        Moderate = 2,
        Severe = 3
    }
}
