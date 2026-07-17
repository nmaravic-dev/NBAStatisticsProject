namespace NBAStatisticsProject.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Season { get; set; } = string.Empty;
        public int HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; } = null!;
        public int AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; } = null!;
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public ICollection<PlayerGameStat> PlayerGameStats { get; set; } = new List<PlayerGameStat>();

    }
}
