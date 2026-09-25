using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class AttributeProductCreateDTO
    {
        public int AttributeProductProductId { get; set; }
        public int AttributeProductAttributesTypeId { get; set; }
        public string? AttributeProductName { get; set; }
        public string? AttributeProductDescription { get; set; }
        public int AttributeProductCreatorId { get; set; }
    }
}
