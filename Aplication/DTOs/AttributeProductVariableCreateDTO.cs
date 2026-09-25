using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class AttributeProductVariableCreateDTO
    {
        public int AttributeProductVariableProductVariableId { get; set; }
        public int AttributeProductVariableAttributeProductId { get; set; }
        public string? AttributeProductVariableValue { get; set; }
        public int AttributeProductVariableCreatorId { get; set; }
    }
}
