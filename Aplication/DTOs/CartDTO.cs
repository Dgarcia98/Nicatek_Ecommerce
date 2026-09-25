using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int CartUserId { get; set; }
        public string? UserName { get; set; }
        public string? UserFullName { get; set; }
        public int CartCreatorId { get; set; }
        public DateTime? CartCreationDate { get; set; }
        public int? CartModificatorId { get; set; }
        public DateTime? CartModificationDate { get; set; }
        public bool CartStatusId { get; set; }
    }
}
