using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConection _conexion;

        public UserRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private User MapearUsuario(SqlDataReader dr)
        {
            return new User
            {
                UserId = Convert.ToInt32(dr["userId"]),
                UserFullName = dr["userFullName"].ToString(),
                UserName = dr["userName"].ToString(),
                UserEmail = dr["userEmail"].ToString(),
                UserPhoneNumber = dr["userPhoneNumber"].ToString(),
                UserCountryId = Convert.ToInt32(dr["userCountryId"]),
                UserGenderId = Convert.ToInt32(dr["userGenderId"]),
                UserBirthDay = Convert.ToDateTime(dr["userBirthDay"]),
                UserStatusId = Convert.ToInt32(dr["userStatusId"])
            };
        }

        public async Task<IEnumerable<User>> ListarUsuariosAsync(bool soloActivos = true)
        {
            var lista = new List<User>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Users", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(MapearUsuario(dr));
            }
            return lista;
        }

        public async Task<IEnumerable<User>> ListarUsuariosFiltroAsync(string filtro, bool soloActivos = true)
        {
            var lista = new List<User>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Users", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(MapearUsuario(dr));
            }
            return lista;
        }

        public async Task<User> LoginAsync(string userName, string password)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Users", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LOGIN"));
            cmd.Parameters.Add(new SqlParameter("@userName", userName));
            cmd.Parameters.Add(new SqlParameter("@userPassword", password));

            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            User? user = null;
            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
                user = MapearUsuario(dr);
            dr.Close();

            if (user == null)
                throw new Exception(oMsg.Value?.ToString() ?? "Credenciales inválidas.");

            return user;
        }

        public async Task NuevoUsuarioAsync(User entity, string password)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Users", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@userFullName", entity.UserFullName));
            cmd.Parameters.Add(new SqlParameter("@userName", entity.UserName));
            cmd.Parameters.Add(new SqlParameter("@userPassword", password));
            cmd.Parameters.Add(new SqlParameter("@userEmail", entity.UserEmail));
            cmd.Parameters.Add(new SqlParameter("@userPhoneNumber", entity.UserPhoneNumber));
            cmd.Parameters.Add(new SqlParameter("@userCountryId", entity.UserCountryId));
            cmd.Parameters.Add(new SqlParameter("@userGenderId", entity.UserGenderId));
            cmd.Parameters.Add(new SqlParameter("@userBirthDay", entity.UserBirthDay));
            cmd.Parameters.Add(new SqlParameter("@userCreatorId", entity.UserCreatorId));

            SqlParameter oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            SqlParameter oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            await cmd.ExecuteNonQueryAsync();
            if ((int)oNum.Value == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarUsuarioAsync(User entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Users", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@userId", entity.UserId));
            cmd.Parameters.Add(new SqlParameter("@userFullName", entity.UserFullName));
            cmd.Parameters.Add(new SqlParameter("@userName", entity.UserName));
            cmd.Parameters.Add(new SqlParameter("@userEmail", entity.UserEmail));
            cmd.Parameters.Add(new SqlParameter("@userPhoneNumber", entity.UserPhoneNumber));
            cmd.Parameters.Add(new SqlParameter("@userCountryId", entity.UserCountryId));
            cmd.Parameters.Add(new SqlParameter("@userGenderId", entity.UserGenderId));
            cmd.Parameters.Add(new SqlParameter("@userBirthDay", entity.UserBirthDay));
            cmd.Parameters.Add(new SqlParameter("@userModificatorId", entity.UserModificatorId));

            SqlParameter oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            SqlParameter oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            await cmd.ExecuteNonQueryAsync();
            if ((int)oNum.Value == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task CambiarPasswordAsync(int userId, string newPassword, int modificatorId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Users", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "PWD"));
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            cmd.Parameters.Add(new SqlParameter("@userPassword", newPassword));
            cmd.Parameters.Add(new SqlParameter("@userModificatorId", modificatorId));

            SqlParameter oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            SqlParameter oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            await cmd.ExecuteNonQueryAsync();
            if ((int)oNum.Value == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task<string> RestablecerPasswordAsync(int userId, int adminUserId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_RestablecerPassword", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            cmd.Parameters.Add(new SqlParameter("@adminUserId", adminUserId));
            var oTmp = new SqlParameter("@passwordTemporal", SqlDbType.NVarChar, 12) { Direction = ParameterDirection.Output };
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oTmp);
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            await cmd.ExecuteNonQueryAsync();

            // El procedimiento valida que quien lo pide sea administrador y que
            // no sea su propia cuenta; ambos rechazos llegan como -1.
            if (Convert.ToInt32(oNum.Value) <= 0)
                throw new Exception(oMsg.Value?.ToString() ?? "No se pudo restablecer la contraseña.");

            return oTmp.Value?.ToString() ?? string.Empty;
        }

        public async Task CambiarPasswordPropiaAsync(int userId, string newPassword)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_CambiarPasswordYLimpiarMarca", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            cmd.Parameters.Add(new SqlParameter("@userPassword", newPassword));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) <= 0)
                throw new Exception(oMsg.Value?.ToString() ?? "No se pudo cambiar la contraseña.");
        }

        public async Task EliminarUsuarioAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Users", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@userId", id));
            cmd.Parameters.Add(new SqlParameter("@userModificatorId", idModificador));

            SqlParameter oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            SqlParameter oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            await cmd.ExecuteNonQueryAsync();
            if ((int)oNum.Value == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}