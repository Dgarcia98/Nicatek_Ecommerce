namespace Aplication.DTOs
{
    public class UserFavoriteDTO
    {
        public int FavoriteId { get; set; }
        public int FavoriteUserId { get; set; }
        public int FavoriteProductId { get; set; }
        public DateTime? FavoriteCreationDate { get; set; }
        public bool FavoriteStatusId { get; set; }
    }
}
