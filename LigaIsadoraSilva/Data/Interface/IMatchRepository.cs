using LigaIsadoraSilva.Data.Entities;

namespace LigaIsadoraSilva.Data.Interface
{
    public interface IMatchRepository
    {
        Task<List<FootballMatch>> GetAllMatchesAsync();
        Task<FootballMatch> GetMatchByIdAsync(int id);
        Task CreateMatchAsync(FootballMatch match);
        Task UpdateMatchAsync(FootballMatch match);
        Task DeleteMatchAsync(int id);
    }
}