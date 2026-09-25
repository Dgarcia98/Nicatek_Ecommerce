using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly DbConection _conexion;

        public RolePermissionRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private RolePermission Mapear(SqlDataReader dr) => new RolePermission
        {
            RolePermissionId = Convert.ToInt32(dr["rolePermissionId"]),
            RolePermissionRoleId = Convert.ToInt32(dr["rolePermissionRoleId"]),
            RoleName = dr["roleName"] != DBNull.Value ? dr["roleName"].ToString() : null,
            RolePermissionPermissionId = Convert.ToInt32(dr["rolePermissionPermissionId"]),
            PermissionName = dr["permissionName"] != DBNull.Value ? dr["permissionName"].ToString() : null,
            PermissionModule = dr["permissionModule"] != DBNull.Value ? dr["permissionModule"].ToString() : null,
            RolePermissionCreatorId = Convert.ToInt32(dr["rolePermissionCreatorId"]),
            RolePermissionCreationDate = dr["rolePermissionCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["rolePermissionCreationDate"]) : null,
            RolePermissionModificatorId = dr["rolePermissionModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["rolePermissionModificatorId"]) : null,
            RolePermissionModificationDate = dr["rolePermissionModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["rolePermissionModificationDate"]) : null,
            RolePermissionStatusId = Convert.ToBoolean(dr["rolePermissionStatusId"])
        };

        public async Task<IEnumerable<RolePermission>> ListarPorRolAsync(int roleId)
        {
            var lista = new List<RolePermission>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_RolePermissions", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@rolePermissionRoleId", roleId));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task AsignarPermisoAsync(RolePermission entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_RolePermissions", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@rolePermissionRoleId", entity.RolePermissionRoleId));
            cmd.Parameters.Add(new SqlParameter("@rolePermissionPermissionId", entity.RolePermissionPermissionId));
            cmd.Parameters.Add(new SqlParameter("@rolePermissionCreatorId", entity.RolePermissionCreatorId));
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
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_RolePermissions", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@rolePermissionId", id));
            cmd.Parameters.Add(new SqlParameter("@rolePermissionModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}