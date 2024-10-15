using LigaIsadoraSilva.Data.Entities;

namespace LigaIsadoraSilva.Data.Interface
{
    public interface ITeamRepository
    {
        void AddFootballTeam(FootballTeam team);

        void DeleteFootballTeam(FootballTeam team);

        FootballTeam GetFootballTeam(int id);

        Task<FootballTeam> GetFootballTeamByIdAsync(int id);

        IEnumerable<FootballTeam> GetFootballTeams();

        Task<bool> SaveAllAsync();

        bool TeamExists(int id);

        void UpdateFootballTeam(FootballTeam team);
    }
}