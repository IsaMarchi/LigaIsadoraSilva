using LigaIsadoraSilva.Data.Entities;

namespace LigaIsadoraSilva.Data.Interface
{
    public interface IMatchService
    {
        Task<List<FootballMatch>> GenerateMatchesAsync(List<FootballTeam> teams);
    }
}