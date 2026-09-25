using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class UserRoleDTO
    {
        public int UserRoleId { get; set; }
        public int UserRoleUserId { get; set; }
        public string? UserFullName { get; set; }
        public string? UserName { get; set; }
        public int UserRoleRoleId { get; set; }
        public string? RoleName { get; set; }
        public int UserRoleCreatorId { get; set; }
        public DateTime? UserRoleCreationDate { get; set; }
        public int? UserRoleModificatorId { get; set; }
        public DateTime? UserRoleModificationDate { get; set; }
        public bool UserRoleStatusId { get; set; }
    }
}
