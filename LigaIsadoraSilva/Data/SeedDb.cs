using LigaIsadoraSilva.Data.Entities;

namespace LigaIsadoraSilva.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Clubs.Any())
            {
                // Add Clubs
                AddClub("Golden Dragons", "Draco Valerion", "Draconic Stadium", "Golden_Dragons.jpeg", new DateTime(1901, 7, 30));
                AddClub("Dragon Fire FC", "Ignatius Pyros", "Arena of Flames", "Dragon_Fire.jpeg", new DateTime(2024, 6, 30));
                AddClub("Royal Dragon", "Rex Draco", "Imperial Stadium", "Royal_Dragon.jpeg", new DateTime(1845, 5, 30));
                AddClub("Dragons Athletic Club", "Boreas Frostwing", "Maritime Arena", "Dragons_Athletic.jpeg", new DateTime(1989, 4, 30));

                await _context.SaveChangesAsync();
            }

            if (!_context.Players.Any())
            {
                // Fetch clubs to link with players
                var teams = _context.Clubs.ToList();

                // Add Players
                AddPlayer("John", "Doe", new DateTime(1990, 1, 15), "American", "john_doe.jpg", teams[0]);
                AddPlayer("Jane", "Smith", new DateTime(1985, 5, 22), "British", "jane_smith.jpg", teams[1]);
                AddPlayer("Carlos", "Mendoza", new DateTime(1992, 11, 10), "Spanish", "carlos_mendoza.jpg", teams[2]);
                AddPlayer("Anna", "Ivanova", new DateTime(1988, 3, 5), "Russian", "anna_ivanova.jpg", teams[3]);

                await _context.SaveChangesAsync();
            }
        }

        private void AddClub(string name, string coach, string stadium, string logo, DateTime foundation)
        {
            _context.Clubs.Add(new FootballTeam
            {
                Name = name,
                Coach = coach,
                Stadium = stadium,
                Photo = logo,
                Foundation = foundation,
                Points = _random.Next(100),
                MatchesPlayed = _random.Next(100),
            });
        }

        private void AddPlayer(string name, string surname, DateTime birth, string nationality, string photo, FootballTeam team)
        {
            _context.Players.Add(new Player
            {
                Name = name,
                Surname = surname,
                Birth = birth,
                Nationality = nationality,
                Photo = photo,
                Club = team
            });
        }
    }
}

