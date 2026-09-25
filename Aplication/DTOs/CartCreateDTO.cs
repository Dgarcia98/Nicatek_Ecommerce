using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CartCreateDTO
    {
        public int CartUserId { get; set; }
        public int CartCreatorId { get; set; }
    }
}
