using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class MarkDTO
    {
        public int MarkId { get; set; }
        public string? MarkName { get; set; }
        public string? MarkDescription { get; set; }
        public int MarkCreatorId { get; set; }
        public DateTime? MarkCreationDate { get; set; }
        public int? MarkModificatorId { get; set; }
        public DateTime? MarkModificationDate { get; set; }
        public bool MarkStatusId { get; set; }
    }
}
