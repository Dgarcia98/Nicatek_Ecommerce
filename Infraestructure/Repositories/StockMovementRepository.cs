using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly DbConection _conexion;

        public StockMovementRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private StockMovement MapearCabecera(SqlDataReader dr) => new StockMovement
        {
            StockMovementId = Convert.ToInt32(dr["stockMovementId"]),
            StockMovementType = Convert.ToInt32(dr["stockMovementType"]),
            StockMovementTypeName = dr["stockMovementTypeName"] != DBNull.Value ? dr["stockMovementTypeName"].ToString() : null,
            StockMovementOrderId = dr["stockMovementOrderId"] != DBNull.Value ? Convert.ToInt32(dr["stockMovementOrderId"]) : null,
            StockMovementReference = dr["stockMovementReference"] != DBNull.Value ? dr["stockMovementReference"].ToString() : null,
            StockMovementDate = Convert.ToDateTime(dr["stockMovementDate"]),
            StockMovementStatusId = Convert.ToInt32(dr["stockMovementStatusId"]),
            StatusName = dr["statusName"] != DBNull.Value ? dr["statusName"].ToString() : null,
            TotalUnidadesMovidas = dr.HasColumn2("totalUnidadesMovidas") ? Convert.ToInt32(dr["totalUnidadesMovidas"]) : 0,
            StockMovementCreatorId = Convert.ToInt32(dr["stockMovementCreatorId"]),
            StockMovementCreationDate = dr["stockMovementCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockMovementCreationDate"]) : null,
            StockMovementModifierId = dr["stockMovementModifierId"] != DBNull.Value ? Convert.ToInt32(dr["stockMovementModifierId"]) : null,
            StockMovementModificationDate = dr["stockMovementModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockMovementModificationDate"]) : null
        };

        private StockMovementDetalle MapearDetalle(SqlDataReader dr) => new StockMovementDetalle
        {
            StockMovementDetailId = Convert.ToInt32(dr["stockMovementDetailId"]),
            StockMovementDetailMovementId = Convert.ToInt32(dr["stockMovementDetailMovementId"]),
            StockMovementDetailOrderDetailId = dr["stockMovementDetailOrderDetailId"] != DBNull.Value ? Convert.ToInt32(dr["stockMovementDetailOrderDetailId"]) : null,
            StockMovementDetailStockId = dr["stockMovementDetailStockId"] != DBNull.Value ? Convert.ToInt32(dr["stockMovementDetailStockId"]) : null,
            StockMovementDetailQuantity = Convert.ToInt32(dr["stockMovementDetailQuantity"]),
            StockMovementDetailFactoryDate = dr["stockMovementDetailFactoryDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockMovementDetailFactoryDate"]) : null,
            StockMovementDetailExpirationDate = dr["stockMovementDetailExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockMovementDetailExpirationDate"]) : null,
            ProductVariableValue = dr["productVariableValue"] != DBNull.Value ? dr["productVariableValue"].ToString() : null,
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            StockMovementDetailStatusId = Convert.ToBoolean(dr["stockMovementDetailStatusId"])
        };

        public async Task<IEnumerable<StockMovement>> ListarAsync(DateTime? desde = null, DateTime? hasta = null)
        {
            var lista = new List<StockMovement>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovements", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@FechaDesde", (object?)desde ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FechaHasta", (object?)hasta ?? DBNull.Value));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearCabecera(dr));
            return lista;
        }

        public async Task<IEnumerable<StockMovement>> FiltrarAsync(string filtro)
        {
            var lista = new List<StockMovement>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovements", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearCabecera(dr));
            return lista;
        }

        public async Task<StockMovementConDetalles?> ObtenerPorIdAsync(int id)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovements", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "GET"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementId", id));

            using var dr = await cmd.ExecuteReaderAsync();

            // Primer resultset: cabecera
            StockMovement? cabecera = null;
            if (await dr.ReadAsync())
                cabecera = MapearCabecera(dr);

            if (cabecera == null) return null;

            var resultado = new StockMovementConDetalles { Cabecera = cabecera };

            // Segundo resultset: detalles
            if (await dr.NextResultAsync())
                while (await dr.ReadAsync())
                    resultado.Detalles.Add(MapearDetalle(dr));

            return resultado;
        }

        public async Task<int> NuevoMovimientoAsync(StockMovement entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovements", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementType", entity.StockMovementType));
            cmd.Parameters.Add(new SqlParameter("@stockMovementOrderId", (object?)entity.StockMovementOrderId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementReference", (object?)entity.StockMovementReference ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDate", entity.StockMovementDate));
            cmd.Parameters.Add(new SqlParameter("@stockMovementCreatorId", entity.StockMovementCreatorId));
            cmd.Parameters.Add(new SqlParameter("@stockMovementStatusId", entity.StockMovementStatusId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            var num = Convert.ToInt32(oNum.Value);
            if (num == -1) throw new Exception(oMsg.Value?.ToString());
            return num; // SCOPE_IDENTITY del nuevo movimiento
        }

        public async Task EditarMovimientoAsync(StockMovement entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovements", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementId", entity.StockMovementId));
            cmd.Parameters.Add(new SqlParameter("@stockMovementReference", (object?)entity.StockMovementReference ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDate", entity.StockMovementDate));
            cmd.Parameters.Add(new SqlParameter("@stockMovementStatusId", entity.StockMovementStatusId));
            cmd.Parameters.Add(new SqlParameter("@stockMovementModifierId", entity.StockMovementModifierId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task AnularMovimientoAsync(int id, int modificadorId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovements", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementId", id));
            cmd.Parameters.Add(new SqlParameter("@stockMovementModifierId", modificadorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }

    // Helper para verificar columna (solo usado aquí)
    internal static class DrHelper
    {
        public static bool HasColumn2(this SqlDataReader dr, string col)
        {
            for (int i = 0; i < dr.FieldCount; i++)
                if (dr.GetName(i).Equals(col, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}