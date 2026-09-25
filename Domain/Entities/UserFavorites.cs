namespace Domain.Entities
{
    public class UserFavorites
    {
        public int FavoriteId { get; set; }
        public int FavoriteUserId { get; set; }
        public int FavoriteProductId { get; set; }
        public DateTime? FavoriteCreationDate { get; set; }
        public bool FavoriteStatusId { get; set; }
    }
}
