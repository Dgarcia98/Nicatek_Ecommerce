using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserAddress
    {
        public int UserAddressId { get; set; }
        public int UserAddressUserId { get; set; }
        public string? UserName { get; set; }
        public int UserAddressCountryId { get; set; }
        public int UserAddressZIPCode { get; set; }
        public string? UserAddressDescription { get; set; }
        public bool UserAddressIsPrincipal { get; set; }
        public int UserAddressCreatorId { get; set; }
        public DateTime? UserAddressCreationDate { get; set; }
        public int? UserAddressModificatorId { get; set; }
        public DateTime? UserAddressModificationDate { get; set; }
        public bool UserAddressStatusId { get; set; }
    }
}
