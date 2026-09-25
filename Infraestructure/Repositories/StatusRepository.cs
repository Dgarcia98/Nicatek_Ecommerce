using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly DbConection _conexion;

        public StatusRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Status MapearStatus(SqlDataReader dr) => new Status
        {
            StatusId = Convert.ToInt32(dr["statusId"]),
            StatusName = dr["statusName"] != DBNull.Value ? dr["statusName"].ToString() : null,
            StatusCreatorId = Convert.ToInt32(dr["statusCreatorId"]),
            StatusCreationDate = dr["statusCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["statusCreationDate"]) : null,
            StatusModificatorId = dr["statusModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["statusModificatorId"]) : null,
            StatusModificationDate = dr["statusModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["statusModificationDate"]) : null,
            StatusStatusId = Convert.ToBoolean(dr["statusStatusId"])
        };

        public async Task<IEnumerable<Status>> ListarStatusAsync()
        {
            var lista = new List<Status>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Status", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearStatus(dr));
            return lista;
        }

        public async Task<IEnumerable<Status>> ListarStatusFiltroAsync(string filtro)
        {
            var lista = new List<Status>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Status", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearStatus(dr));
            return lista;
        }

        public async Task NuevoStatusAsync(Status oStatus)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Status", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@statusName", oStatus.StatusName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@statusCreatorId", oStatus.StatusCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarStatusAsync(Status oStatus)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Status", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@statusId", oStatus.StatusId));
            cmd.Parameters.Add(new SqlParameter("@statusName", oStatus.StatusName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@statusModificatorId", oStatus.StatusModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarStatusAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Status", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@statusId", id));
            cmd.Parameters.Add(new SqlParameter("@statusModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}