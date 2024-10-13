using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Data.Interface;
using Microsoft.AspNetCore.Components.Web;

namespace LigaIsadoraSilva.Data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DataContext _context;
        public TeamRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<FootballTeam> GetFootballTeams()
        {
            return _context.Clubs.OrderBy(p => p.Name); //ordena os times pelo nome
        }

        public FootballTeam GetFootballTeam(int id)
        {
            return _context.Clubs.Find(id);
        }

        public void AddFootballTeam(FootballTeam team)
        {
            _context.Clubs.Add(team);
        }

        public void UpdateFootballTeam(FootballTeam team)
        {
            _context.Clubs.Update(team);
        }

        public void DeleteFootballTeam(FootballTeam team)
        {
            _context.Clubs.Remove(team);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool TeamExists(int id)
        {
            return _context.Clubs.Any(p => p.Id == id);
        }
    }
}
