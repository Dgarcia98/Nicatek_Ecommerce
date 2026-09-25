using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductVariableTypes
    {
        public int ProductVariableTypeId { get; set; }
        public string? ProductVariableTypeName { get; set; }
        public string? ProductVariableTypeDescription { get; set; }
        public int ProductVariableTypeCreatorId { get; set; }
        public DateTime? ProductVariableTypeCreationDate { get; set; }
        public int? ProductVariableTypeModificatorId { get; set; }
        public DateTime? ProductVariableTypeModificationDate { get; set; }
        public bool ProductVariableTypeStatusId { get; set; }
    }
}
