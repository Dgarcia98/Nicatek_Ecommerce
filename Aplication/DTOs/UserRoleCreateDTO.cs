using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class UserRoleCreateDTO
    {
        public int UserRoleUserId { get; set; }
        public int UserRoleRoleId { get; set; }
        public int UserRoleCreatorId { get; set; }
    }
}
