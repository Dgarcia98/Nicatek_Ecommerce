using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class AttributeProductUpdateDTO
    {
        public int AttributeProductId { get; set; }
        public int AttributeProductProductId { get; set; }
        public int AttributeProductAttributesTypeId { get; set; }
        public string? AttributeProductName { get; set; }
        public string? AttributeProductDescription { get; set; }
        public int AttributeProductModificatorId { get; set; }
    }
}
