using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ProductImageCreateDTO
    {
        public int ProductImageProductId { get; set; }
        public string? ProductImageURL { get; set; }
        public string? ProductImageDescription { get; set; }
        public bool ProductImageIsPrincipal { get; set; }
        public int ProductImageCreatorId { get; set; }
    }
}
