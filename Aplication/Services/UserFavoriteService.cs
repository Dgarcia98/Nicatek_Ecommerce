using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class UserFavoriteService : IUserFavoriteService
    {
        private readonly IUserFavoriteRepository _repository;
        public UserFavoriteService(IUserFavoriteRepository repository) => _repository = repository;

        private UserFavoriteDTO ToDTO(UserFavorites item) => new UserFavoriteDTO
        {
            FavoriteId           = item.FavoriteId,
            FavoriteUserId       = item.FavoriteUserId,
            FavoriteProductId    = item.FavoriteProductId,
            FavoriteCreationDate = item.FavoriteCreationDate,
            FavoriteStatusId     = item.FavoriteStatusId,
        };

        public async Task<IEnumerable<UserFavoriteDTO>> ListarFavoritos(int userId)
            => (await _repository.ListarFavoritosAsync(userId)).Select(ToDTO);

        public async Task<int?> VerificarFavorito(int userId, int productId)
            => await _repository.VerificarFavoritoAsync(userId, productId);

        public async Task<int> AgregarFavorito(int userId, int productId)
            => await _repository.AgregarFavoritoAsync(userId, productId);

        public async Task QuitarFavorito(int favoriteId)
            => await _repository.QuitarFavoritoAsync(favoriteId);
    }
}
