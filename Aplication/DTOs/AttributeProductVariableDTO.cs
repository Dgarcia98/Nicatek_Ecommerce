using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class AttributeProductVariableDTO
    {
        public int AttributeProductVariableId { get; set; }
        public int AttributeProductVariableProductVariableId { get; set; }
        public string? ProductVariableValue { get; set; }
        public string? ProductName { get; set; }
        public int AttributeProductVariableAttributeProductId { get; set; }
        public string? ProductVariableTypeName { get; set; }
        public string? AttributeProductVariableValue { get; set; }
        public int AttributeProductVariableCreatorId { get; set; }
        public DateTime? AttributeProductVariableCreationDate { get; set; }
        public int? AttributeProductVariableModificatorId { get; set; }
        public DateTime? AttributeProductVariableModificationDate { get; set; }
        public bool AttributeProductVariableStatusId { get; set; }
    }
}
