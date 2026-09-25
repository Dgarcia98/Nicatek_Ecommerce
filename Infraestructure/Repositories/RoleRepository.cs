using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbConection _conexion;

        public RoleRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Roles MapearRole(SqlDataReader dr) => new Roles
        {
            RoleId = Convert.ToInt32(dr["roleId"]),
            RoleName = dr["roleName"] != DBNull.Value ? dr["roleName"].ToString() : null,
            RoleDescription = dr["roleDescription"] != DBNull.Value ? dr["roleDescription"].ToString() : null,
            RoleCreatorId = Convert.ToInt32(dr["roleCreatorId"]),
            RoleCreationDate = dr["roleCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["roleCreationDate"]) : null,
            RoleModificatorId = dr["roleModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["roleModificatorId"]) : null,
            RoleModificationDate = dr["roleModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["roleModificationDate"]) : null,
            RoleStatusId = Convert.ToBoolean(dr["roleStatusId"])
        };

        public async Task<IEnumerable<Roles>> ListarRolesAsync()
        {
            var lista = new List<Roles>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Roles", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearRole(dr));
            return lista;
        }

        public async Task<IEnumerable<Roles>> ListarRolesFiltroAsync(string filtro)
        {
            var lista = new List<Roles>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Roles", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearRole(dr));
            return lista;
        }

        public async Task NuevoRoleAsync(Roles oRole)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Roles", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@roleName", oRole.RoleName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@roleDescription", oRole.RoleDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@roleCreatorId", oRole.RoleCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarRoleAsync(Roles oRole)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Roles", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@roleId", oRole.RoleId));
            cmd.Parameters.Add(new SqlParameter("@roleName", oRole.RoleName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@roleDescription", oRole.RoleDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@roleModificatorId", oRole.RoleModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarRoleAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Roles", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@roleId", id));
            cmd.Parameters.Add(new SqlParameter("@roleModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}