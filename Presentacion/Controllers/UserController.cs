using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            try
            {
                var user = await _service.LoginAsync(dto);
                return Ok(new { msj = "Autenticación exitosa.", data = user });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { msj = ex.Message });
            }
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar([FromQuery] bool soloActivos = true)
        {
            try
            {
                var lista = await _service.ListarUsuariosAsync(soloActivos);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msj = ex.Message });
            }
        }

        [HttpGet("ListarFiltro")]
        public async Task<IActionResult> ListarFiltro([FromQuery] string filtro, [FromQuery] bool soloActivos = true)
        {
            try
            {
                var lista = await _service.ListarUsuariosFiltroAsync(filtro, soloActivos);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msj = ex.Message });
            }
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] UserCreateDTO dto)
        {
            try
            {
                await _service.NuevoUsuarioAsync(dto);
                return Ok(new { msj = "Usuario registrado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msj = ex.Message });
            }
        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] UserUpdateDTO dto)
        {
            try
            {
                await _service.EditarUsuarioAsync(dto);
                return Ok(new { msj = "Datos de usuario actualizados correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msj = ex.Message });
            }
        }

        /// <summary>
        /// Genera una clave temporal para un cliente que perdio la suya.
        ///
        /// La temporal se devuelve UNA vez, para que el administrador se la
        /// dicte. No queda accesible despues: el procedimiento la guarda
        /// cifrada y marca la cuenta para que el cliente deba cambiarla.
        /// </summary>
        [HttpPost("RestablecerPassword")]
        public async Task<IActionResult> RestablecerPassword([FromBody] RestablecerPasswordDTO dto)
        {
            try
            {
                var temporal = await _service.RestablecerPasswordAsync(dto);
                return Ok(new
                {
                    passwordTemporal = temporal,
                    msj = "Contraseña restablecida. El cliente deberá cambiarla al iniciar sesión.",
                });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        /// <summary>Cambio por el propio usuario; retira la marca de cambio obligatorio.</summary>
        [HttpPut("CambiarPasswordPropia")]
        public async Task<IActionResult> CambiarPasswordPropia([FromBody] CambiarPasswordPropiaDTO dto)
        {
            try
            {
                await _service.CambiarPasswordPropiaAsync(dto);
                return Ok(new { msj = "Contraseña actualizada correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("CambiarPassword")]
        public async Task<IActionResult> CambiarPassword([FromBody] UserChangePasswordDTO dto)
        {
            try
            {
                await _service.CambiarPasswordAsync(dto);
                return Ok(new { msj = "Contraseña modificada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msj = ex.Message });
            }
        }

        [HttpDelete("Eliminar")]
        public async Task<IActionResult> Eliminar(int id, int idModificador)
        {
            try
            {
                await _service.EliminarUsuarioAsync(id, idModificador);
                return Ok(new { msj = "Estado del usuario actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msj = ex.Message });
            }
        }
    }
}