using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class UserChangePasswordDTO
    {
        public int UserId { get; set; }
        public string? UserPassword { get; set; }
        public int UserModificatorId { get; set; }
    }
}
