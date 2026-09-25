using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class AttributeProductDTO
    {
        public int AttributeProductId { get; set; }
        public int AttributeProductProductId { get; set; }
        public string? ProductName { get; set; }
        public int AttributeProductAttributesTypeId { get; set; }
        public string? AttributeTypeName { get; set; }
        public string? AttributeProductName { get; set; }
        public string? AttributeProductDescription { get; set; }
        public int AttributeProductCreatorId { get; set; }
        public DateTime? AttributeProductCreationDate { get; set; }
        public int? AttributeProductModificatorId { get; set; }
        public DateTime? AttributeProductModificationDate { get; set; }
        public bool AttributeProductStatusId { get; set; }
    }
}
