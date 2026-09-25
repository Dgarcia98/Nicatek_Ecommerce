using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        private UserDTO ToDTO(User x) => new UserDTO
        {
            UserId = x.UserId,
            UserFullName = x.UserFullName,
            UserName = x.UserName,
            UserEmail = x.UserEmail,
            UserPhoneNumber = x.UserPhoneNumber,
            UserCountryId = x.UserCountryId,
            UserGenderId = x.UserGenderId,
            UserBirthDay = x.UserBirthDay,
            UserStatusId = x.UserStatusId
        };

        private string GenerarToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name,           user.UserName     ?? string.Empty),
                new Claim(ClaimTypes.Email,          user.UserEmail    ?? string.Empty),
                new Claim("FullName",                user.UserFullName ?? string.Empty),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IEnumerable<UserDTO>> ListarUsuariosAsync(bool soloActivos = true)
        {
            var lista = await _repository.ListarUsuariosAsync(soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<UserDTO>> ListarUsuariosFiltroAsync(string filtro, bool soloActivos = true)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Enumerable.Empty<UserDTO>();
            var lista = await _repository.ListarUsuariosFiltroAsync(filtro, soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<UserLoginResponseDTO> LoginAsync(UserLoginDTO dto)
        {
            var user = await _repository.LoginAsync(dto.UserName!, dto.UserPassword!);
            var token = GenerarToken(user);

            return new UserLoginResponseDTO
            {
                Token = token,
                UserId = user.UserId,
                UserName = user.UserName ?? string.Empty,
                UserFullName = user.UserFullName ?? string.Empty,
                UserEmail = user.UserEmail ?? string.Empty,
                UserStatusId = user.UserStatusId
            };
        }

        public async Task NuevoUsuarioAsync(UserCreateDTO dto)
        {
            var entity = new User
            {
                UserFullName = dto.UserFullName,
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                UserPhoneNumber = dto.UserPhoneNumber,
                UserCountryId = dto.UserCountryId,
                UserGenderId = dto.UserGenderId,
                UserBirthDay = dto.UserBirthDay,
                UserCreatorId = dto.UserCreatorId
            };
            await _repository.NuevoUsuarioAsync(entity, dto.UserPassword!);
        }

        public async Task EditarUsuarioAsync(UserUpdateDTO dto)
        {
            var entity = new User
            {
                UserId = dto.UserId,
                UserFullName = dto.UserFullName,
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                UserPhoneNumber = dto.UserPhoneNumber,
                UserCountryId = dto.UserCountryId,
                UserGenderId = dto.UserGenderId,
                UserBirthDay = dto.UserBirthDay,
                UserModificatorId = dto.UserModificatorId
            };
            await _repository.EditarUsuarioAsync(entity);
        }

        public Task<string> RestablecerPasswordAsync(RestablecerPasswordDTO dto)
            => _repository.RestablecerPasswordAsync(dto.UserId, dto.AdminUserId);

        public async Task CambiarPasswordPropiaAsync(CambiarPasswordPropiaDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NuevaPassword) || dto.NuevaPassword.Trim().Length < 8)
                throw new Exception("La contraseña debe tener al menos 8 caracteres.");
            await _repository.CambiarPasswordPropiaAsync(dto.UserId, dto.NuevaPassword.Trim());
        }

        public async Task CambiarPasswordAsync(UserChangePasswordDTO dto)
        {
            await _repository.CambiarPasswordAsync(dto.UserId, dto.UserPassword!, dto.UserModificatorId);
        }

        public async Task EliminarUsuarioAsync(int id, int idModificador)
        {
            await _repository.EliminarUsuarioAsync(id, idModificador);
        }
    }
}