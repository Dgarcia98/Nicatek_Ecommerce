using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class RolePermissionCreateDTO
    {
        public int RolePermissionRoleId { get; set; }
        public int RolePermissionPermissionId { get; set; }
        public int RolePermissionCreatorId { get; set; }
    }
}
