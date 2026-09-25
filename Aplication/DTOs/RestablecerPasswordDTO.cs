namespace Aplication.DTOs
{
    /// <summary>Peticion del administrador para restablecer la clave de un cliente.</summary>
    public class RestablecerPasswordDTO
    {
        public int UserId { get; set; }
        /// <summary>Quien lo pide. El procedimiento verifica que tenga rol de administrador.</summary>
        public int AdminUserId { get; set; }
    }

    /// <summary>Cambio de contraseña por el propio usuario tras un restablecimiento.</summary>
    public class CambiarPasswordPropiaDTO
    {
        public int UserId { get; set; }
        public string? NuevaPassword { get; set; }
    }
}
