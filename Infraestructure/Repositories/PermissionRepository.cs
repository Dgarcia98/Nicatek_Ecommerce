using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DbConection _conexion;

        public PermissionRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Permissions MapearPermission(SqlDataReader dr) => new Permissions
        {
            PermissionId = Convert.ToInt32(dr["permissionId"]),
            PermissionName = dr["permissionName"] != DBNull.Value ? dr["permissionName"].ToString() : null,
            PermissionDescription = dr["permissionDescription"] != DBNull.Value ? dr["permissionDescription"].ToString() : null,
            PermissionModule = dr["permissionModule"] != DBNull.Value ? dr["permissionModule"].ToString() : null,
            PermissionCreatorId = Convert.ToInt32(dr["permissionCreatorId"]),
            PermissionCreationDate = dr["permissionCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["permissionCreationDate"]) : null,
            PermissionModificatorId = dr["permissionModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["permissionModificatorId"]) : null,
            PermissionModificationDate = dr["permissionModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["permissionModificationDate"]) : null,
            PermissionStatusId = Convert.ToBoolean(dr["permissionStatusId"])
        };

        public async Task<IEnumerable<Permissions>> ListarPermissionsAsync()
        {
            var lista = new List<Permissions>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Permissions", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearPermission(dr));
            return lista;
        }

        public async Task<IEnumerable<Permissions>> ListarPermissionsFiltroAsync(string filtro)
        {
            var lista = new List<Permissions>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Permissions", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearPermission(dr));
            return lista;
        }

        public async Task NuevoPermissionAsync(Permissions oPermission)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Permissions", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@permissionName", oPermission.PermissionName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@permissionDescription", oPermission.PermissionDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@permissionModule", oPermission.PermissionModule ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@permissionCreatorId", oPermission.PermissionCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarPermissionAsync(Permissions oPermission)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Permissions", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@permissionId", oPermission.PermissionId));
            cmd.Parameters.Add(new SqlParameter("@permissionName", oPermission.PermissionName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@permissionDescription", oPermission.PermissionDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@permissionModule", oPermission.PermissionModule ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@permissionModificatorId", oPermission.PermissionModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarPermissionAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_SECURITY.sp_Tbl_Permissions", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@permissionId", id));
            cmd.Parameters.Add(new SqlParameter("@permissionModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}