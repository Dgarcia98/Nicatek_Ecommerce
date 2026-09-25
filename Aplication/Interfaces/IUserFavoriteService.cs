using Aplication.DTOs;

namespace Aplication.Interfaces
{
    public interface IUserFavoriteService
    {
        Task<IEnumerable<UserFavoriteDTO>> ListarFavoritos(int userId);
        Task<int?> VerificarFavorito(int userId, int productId);
        Task<int> AgregarFavorito(int userId, int productId);
        Task QuitarFavorito(int favoriteId);
    }
}
