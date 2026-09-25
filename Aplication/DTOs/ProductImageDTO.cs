using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ProductImageDTO
    {
        public int ProductImageId { get; set; }
        public int ProductImageProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImageURL { get; set; }
        public string? ProductImageDescription { get; set; }
        public bool ProductImageIsPrincipal { get; set; }
        public int ProductImageCreatorId { get; set; }
        public DateTime? ProductImageCreationDate { get; set; }
        public int? ProductImageModificatorId { get; set; }
        public DateTime? ProductImageModificationDate { get; set; }
        public bool ProductImageStatusId { get; set; }
    }
}
