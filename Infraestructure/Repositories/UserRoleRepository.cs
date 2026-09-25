using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly DbConection _conexion;

        public UserRoleRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private UserRole Mapear(SqlDataReader dr) => new UserRole
        {
            UserRoleId = Convert.ToInt32(dr["userRoleId"]),
            UserRoleUserId = Convert.ToInt32(dr["userRoleUserId"]),
            UserFullName = dr["userFullName"] != DBNull.Value ? dr["userFullName"].ToString() : null,
            UserName = dr["userName"] != DBNull.Value ? dr["userName"].ToString() : null,
            UserRoleRoleId = Convert.ToInt32(dr["userRoleRoleId"]),
            RoleName = dr["roleName"] != DBNull.Value ? dr["roleName"].ToString() : null,
            UserRoleCreatorId = Convert.ToInt32(dr["userRoleCreatorId"]),
            UserRoleCreationDate = dr["userRoleCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["userRoleCreationDate"]) : null,
            UserRoleModificatorId = dr["userRoleModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["userRoleModificatorId"]) : null,
            UserRoleModificationDate = dr["userRoleModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["userRoleModificationDate"]) : null,
            UserRoleStatusId = Convert.ToBoolean(dr["userRoleStatusId"])
        };

        public async Task<IEnumerable<UserRole>> ListarPorUsuarioAsync(int userId)
        {
            var lista = new List<UserRole>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_UserRoles", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@userRoleUserId", userId));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<IEnumerable<int>> ListarUserIdsConRolesAsync()
        {
            var lista = new List<int>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand(
                "SELECT DISTINCT userRoleUserId FROM [SQM_SECURITY].[Tbl_UserRoles] WHERE userRoleStatusId = 1",
                con);
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Convert.ToInt32(dr[0]));
            return lista;
        }

        public async Task AsignarRolAsync(UserRole entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_UserRoles", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@userRoleUserId", entity.UserRoleUserId));
            cmd.Parameters.Add(new SqlParameter("@userRoleRoleId", entity.UserRoleRoleId));
            cmd.Parameters.Add(new SqlParameter("@userRoleCreatorId", entity.UserRoleCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarAsignacionAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_UserRoles", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@userRoleId", id));
            cmd.Parameters.Add(new SqlParameter("@userRoleModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}