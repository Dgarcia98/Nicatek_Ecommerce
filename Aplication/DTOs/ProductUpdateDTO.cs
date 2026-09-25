using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ProductUpdateDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int ProductProductIdentificatorId { get; set; }
        public int ProductMarkByProviderId { get; set; }
        public int ProductModificatorId { get; set; }
    }
}
