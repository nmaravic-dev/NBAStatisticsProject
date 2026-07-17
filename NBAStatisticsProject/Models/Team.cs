namespace NBAStatisticsProject.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ICollection<Game> HomeGames { get; set; } = new List<Game>();
        public ICollection<Game> AwayGames { get; set; } = new List<Game>();
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
