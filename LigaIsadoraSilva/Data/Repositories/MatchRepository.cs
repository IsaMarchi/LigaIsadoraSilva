using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace LigaIsadoraSilva.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly DataContext _context;

        public MatchRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<FootballMatch>> GetAllMatchesAsync()
        {
            return await _context.Games
                .Include(m => m.HomeTeam)
                .Include(m => m.VisitTeam)
                .OrderByDescending(m => m.StartDate) // Ordena as partidas por data, mais recentes primeiro
                .ToListAsync();
        }

        public async Task<FootballMatch> GetMatchByIdAsync(int id)
        {
            return await _context.Games
                .Include(m => m.HomeTeam)
                .Include(m => m.VisitTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateMatchAsync(FootballMatch match)
        {
            _context.Games.Add(match);
            await SaveChangesWithErrorHandling();
        }

        public async Task UpdateMatchAsync(FootballMatch match)
        {
            // Não é necessário chamar Update() se a entidade já está sendo rastreada
            _context.Entry(match).State = EntityState.Modified; // Marcar a entidade como modificada
            await SaveChangesWithErrorHandling();
        }

        public async Task DeleteMatchAsync(int id)
        {
            var match = await _context.Games.FindAsync(id);
            if (match != null)
            {
                _context.Games.Remove(match);
                await SaveChangesWithErrorHandling();
            }
        }

        private async Task SaveChangesWithErrorHandling()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao salvar no banco de dados: {ex.Message}");
                throw; // Re-lançar a exceção após o log
            }
        }
    }
}

