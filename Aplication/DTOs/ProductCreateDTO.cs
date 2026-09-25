using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ProductCreateDTO
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int ProductProductIdentificatorId { get; set; }
        public int ProductMarkByProviderId { get; set; }
        public int ProductCreatorId { get; set; }
    }

}
