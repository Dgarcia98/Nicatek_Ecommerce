using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Status
    {
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public int StatusCreatorId { get; set; }
        public DateTime? StatusCreationDate { get; set; }
        public int? StatusModificatorId { get; set; }
        public DateTime? StatusModificationDate { get; set; }
        public bool StatusStatusId { get; set; }
    }
}
