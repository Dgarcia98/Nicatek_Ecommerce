using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class UserAddressCreateDTO
    {
        public int UserAddressUserId { get; set; }
        public int UserAddressCountryId { get; set; }
        public int UserAddressZIPCode { get; set; }
        public string? UserAddressDescription { get; set; }
        public bool UserAddressIsPrincipal { get; set; }
        public int UserAddressCreatorId { get; set; }
    }
}
