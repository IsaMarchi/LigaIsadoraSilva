using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace LigaIsadoraSilva.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DataContext _context;
        public PlayerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Player> GetPlayerByIdWithClubs(int? id)
        {
            if (id == null)
            {
                //view not found
                return null;
            }

            var player = await _context.Players
                .Include(s => s.Club)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (player == null)
            {
                //view not found
                return null;
            }

            return player;
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
