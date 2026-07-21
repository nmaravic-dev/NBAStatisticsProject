namespace NBAStatisticsProject.Models
{
    public class WatchlistEntry
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }
        public int PlayerId { get; set; }
        public Player? Player { get; set; }
    }
}
