using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class RolePermissionDTO
    {
        public int RolePermissionId { get; set; }
        public int RolePermissionRoleId { get; set; }
        public string? RoleName { get; set; }
        public int RolePermissionPermissionId { get; set; }
        public string? PermissionName { get; set; }
        public string? PermissionModule { get; set; }
        public int RolePermissionCreatorId { get; set; }
        public DateTime? RolePermissionCreationDate { get; set; }
        public int? RolePermissionModificatorId { get; set; }
        public DateTime? RolePermissionModificationDate { get; set; }
        public bool RolePermissionStatusId { get; set; }
    }

}
