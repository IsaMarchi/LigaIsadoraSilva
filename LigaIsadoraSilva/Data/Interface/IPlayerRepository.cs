using LigaIsadoraSilva.Data.Entities;

namespace LigaIsadoraSilva.Data.Interface
{
    public interface IPlayerRepository
    {
        void AddPlayers(Player player);

        void DeletePlayer(Player player);

        IEnumerable<Player> GetPlayers();

        Player GetPlayers(int id);

        bool PlayerExists(int id);

        Task<bool> SaveAllAsync();

        void UpdatePlayer(Player player);
    }
}