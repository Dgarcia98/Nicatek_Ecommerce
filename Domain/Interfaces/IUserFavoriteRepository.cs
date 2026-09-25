using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserFavoriteRepository
    {
        Task<IEnumerable<UserFavorites>> ListarFavoritosAsync(int userId);
        Task<int?> VerificarFavoritoAsync(int userId, int productId);
        Task<int> AgregarFavoritoAsync(int userId, int productId);
        Task QuitarFavoritoAsync(int favoriteId);
    }
}
