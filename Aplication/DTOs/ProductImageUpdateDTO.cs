using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ProductImageUpdateDTO
    {
        public int ProductImageId { get; set; }
        public string? ProductImageURL { get; set; }
        public string? ProductImageDescription { get; set; }
        public bool ProductImageIsPrincipal { get; set; }
        public int ProductImageModificatorId { get; set; }
    }
}
