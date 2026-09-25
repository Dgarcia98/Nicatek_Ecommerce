using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int ProductProductIdentificatorId { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string? SegmentName { get; set; }
        public int ProductMarkByProviderId { get; set; }
        public string? MarkName { get; set; }
        public string? ProviderName { get; set; }
        public int ProductCreatorId { get; set; }
        public DateTime? ProductCreationDate { get; set; }
        public int? ProductModificatorId { get; set; }
        public DateTime? ProductModificationDate { get; set; }
        public bool ProductStatusId { get; set; }
        public string? ProductImageURL { get; set; }
    }
}
