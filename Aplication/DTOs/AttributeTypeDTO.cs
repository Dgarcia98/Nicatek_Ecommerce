using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class AttributeTypeDTO
    {
        public int AttributeTypeId { get; set; }
        public string? AttributeTypeName { get; set; }
        public string? AttributeTypeDescription { get; set; }
        public int AttributeTypeCreatorId { get; set; }
        public DateTime? AttributeTypeCreationDate { get; set; }
        public int? AttributeTypeModificatorId { get; set; }
        public DateTime? AttributeTypeModificationDate { get; set; }
        public bool AttributeTypeStatusId { get; set; }
    }
}
