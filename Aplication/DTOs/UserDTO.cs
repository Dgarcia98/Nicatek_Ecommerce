using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    // Usado para listar usuarios (NUNCA expone el password)
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public int UserCountryId { get; set; }
        public int UserGenderId { get; set; }
        public DateTime UserBirthDay { get; set; }
        public int UserStatusId { get; set; }
    }
}
