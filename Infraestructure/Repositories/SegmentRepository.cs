using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class SegmentRepository : ISegmentRepository
    {
        private readonly DbConection _conexion;

        public SegmentRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Segments MapearSegment(SqlDataReader dr) => new Segments
        {
            SegmentId = Convert.ToInt32(dr["segmentId"]),
            SegmentName = dr["segmentName"] != DBNull.Value ? dr["segmentName"].ToString() : null,
            SegmentDescription = dr["segmentDescription"] != DBNull.Value ? dr["segmentDescription"].ToString() : null,
            SegmentCreatorId = Convert.ToInt32(dr["segmentCreatorId"]),
            SegmentCreationDate = dr["segmentCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["segmentCreationDate"]) : null,
            SegmentModificatorId = dr["segmentModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["segmentModificatorId"]) : null,
            SegmentModificationDate = dr["segmentModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["segmentModificationDate"]) : null,
            SegmentStatusId = Convert.ToBoolean(dr["segmentStatusId"])
        };

        public async Task<IEnumerable<Segments>> ListarSegmentsAsync(bool soloActivos = true)
        {
            var lista = new List<Segments>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Segments", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearSegment(dr));
            return lista;
        }

        public async Task<IEnumerable<Segments>> ListarSegmentsFiltroAsync(string filtro)
        {
            var lista = new List<Segments>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Segments", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearSegment(dr));
            return lista;
        }

        public async Task NuevoSegmentAsync(Segments oSegment)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Segments", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@segmentName", oSegment.SegmentName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@segmentDescription", oSegment.SegmentDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@segmentCreatorId", oSegment.SegmentCreatorId));
            var oMsgIns = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumIns = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgIns);
            cmd.Parameters.Add(oNumIns);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumIns.Value) == -1) throw new Exception(oMsgIns.Value?.ToString());
        }

        public async Task EditarSegmentAsync(Segments oSegment)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Segments", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@segmentId", oSegment.SegmentId));
            cmd.Parameters.Add(new SqlParameter("@segmentName", oSegment.SegmentName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@segmentDescription", oSegment.SegmentDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@segmentModificatorId", oSegment.SegmentModificatorId ?? (object)DBNull.Value));
            var oMsgUpd = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumUpd = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgUpd);
            cmd.Parameters.Add(oNumUpd);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumUpd.Value) == -1) throw new Exception(oMsgUpd.Value?.ToString());
        }

        public async Task EliminarSegmentAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Segments", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@segmentId", id));
            cmd.Parameters.Add(new SqlParameter("@segmentModificatorId", idModificador));
            var oMsgDel = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumDel = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgDel);
            cmd.Parameters.Add(oNumDel);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumDel.Value) == -1) throw new Exception(oMsgDel.Value?.ToString());
        }
    }
}