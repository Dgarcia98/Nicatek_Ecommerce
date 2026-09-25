using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ProductIdentificatorDTO
    {
        public int ProductIdentificatorId { get; set; }
        public int ProductIdentificatorCategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int ProductIdentificatorSubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public int ProductIdentificatorSegmentId { get; set; }
        public string? SegmentName { get; set; }
        public int ProductIdentificatorCreatorId { get; set; }
        public DateTime? ProductIdentificatorCreationDate { get; set; }
        public int? ProductIdentificatorModificatorId { get; set; }
        public DateTime? ProductIdentificatorModificationDate { get; set; }
        public bool ProductIdentificatorStatusId { get; set; }
    }
}
