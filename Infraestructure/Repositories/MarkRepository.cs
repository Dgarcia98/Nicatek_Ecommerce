using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class MarkRepository : IMarkRepository
    {
        private readonly DbConection _conexion;

        public MarkRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Marks MapearMark(SqlDataReader dr) => new Marks
        {
            MarkId = Convert.ToInt32(dr["markId"]),
            MarkName = dr["markName"] != DBNull.Value ? dr["markName"].ToString() : null,
            MarkDescription = dr["markDescription"] != DBNull.Value ? dr["markDescription"].ToString() : null,
            MarkCreatorId = Convert.ToInt32(dr["markCreatorId"]),
            MarkCreationDate = dr["markCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["markCreationDate"]) : null,
            MarkModificatorId = dr["markModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["markModificatorId"]) : null,
            MarkModificationDate = dr["markModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["markModificationDate"]) : null,
            MarkStatusId = Convert.ToBoolean(dr["markStatusId"])
        };

        public async Task<IEnumerable<Marks>> ListarMarksAsync()
        {
            var lista = new List<Marks>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Marks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearMark(dr));
            return lista;
        }

        public async Task<IEnumerable<Marks>> ListarMarksFiltroAsync(string filtro)
        {
            var lista = new List<Marks>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Marks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearMark(dr));
            return lista;
        }

        public async Task NuevoMarkAsync(Marks oMark)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Marks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@markName", oMark.MarkName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@markDescription", oMark.MarkDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@markCreatorId", oMark.MarkCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarMarkAsync(Marks oMark)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Marks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@markId", oMark.MarkId));
            cmd.Parameters.Add(new SqlParameter("@markName", oMark.MarkName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@markDescription", oMark.MarkDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@markModificatorId", oMark.MarkModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarMarkAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Marks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@markId", id));
            cmd.Parameters.Add(new SqlParameter("@markModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}