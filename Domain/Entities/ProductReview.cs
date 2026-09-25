namespace Domain.Entities
{
    /// <summary>
    /// Reseña de un producto. Solo puede dejarla quien compró el producto y ya
    /// lo recibió, de modo que las opiniones sean de compras verificadas.
    /// </summary>
    public class ProductReview
    {
        public int       ReviewId               { get; set; }
        public int       ReviewProductId        { get; set; }
        public int       ReviewUserId           { get; set; }
        public string?   ReviewUserName         { get; set; }   // viene del JOIN con Usuarios
        public byte      ReviewRating           { get; set; }   // 1 a 5
        public string?   ReviewComment          { get; set; }
        public DateTime? ReviewCreationDate     { get; set; }
        public DateTime? ReviewModificationDate { get; set; }
        public bool      ReviewStatusId         { get; set; }
    }

    /// <summary>Promedio y desglose por estrella de un producto.</summary>
    public class ProductReviewSummary
    {
        public decimal AverageRating { get; set; }
        public int     TotalReviews  { get; set; }
        public int     Count5        { get; set; }
        public int     Count4        { get; set; }
        public int     Count3        { get; set; }
        public int     Count2        { get; set; }
        public int     Count1        { get; set; }
    }

    /// <summary>Si un usuario puede reseñar un producto y si ya lo hizo.</summary>
    public class ReviewEligibility
    {
        public bool HasPurchased { get; set; }
        public bool HasReviewed  { get; set; }
    }
}
