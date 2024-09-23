using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Data.Interface;

namespace LigaIsadoraSilva.Data
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DataContext _context;
        public PlayerRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Player> GetPlayers()
        {
            return _context.Players.OrderBy(p => p.Name); //ordena os jogadores pelo nome
        }

        public Player GetPlayers(int id)
        {
            return _context.Players.Find(id);
        }

        public void AddPlayers(Player player)
        {
            _context.Players.Add(player);
        }

        public void UpdatePlayer(Player player)
        {
            _context.Players.Update(player);
        }

        public void DeletePlayer(Player player)
        {
            _context.Players.Remove(player);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool PlayerExists(int id)
        {
            return _context.Players.Any(p => p.Id == id);
        }
    }
}
