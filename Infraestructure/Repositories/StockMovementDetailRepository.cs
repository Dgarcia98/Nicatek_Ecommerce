using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class StockMovementDetailRepository : IStockMovementDetailRepository
    {
        private readonly DbConection _conexion;

        public StockMovementDetailRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private StockMovementDetail Mapear(SqlDataReader dr) => new StockMovementDetail
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
            MarkName = dr["markName"] != DBNull.Value ? dr["markName"].ToString() : null,
            CurrencyISO = dr["currencyISO"] != DBNull.Value ? dr["currencyISO"].ToString() : null,
            ProductVariablePrice = dr["productVariablePrice"] != DBNull.Value ? Convert.ToDecimal(dr["productVariablePrice"]) : null,
            StockMovementDetailCreatorId = Convert.ToInt32(dr["stockMovementDetailCreatorId"]),
            StockMovementDetailCreationDate = dr["stockMovementDetailCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockMovementDetailCreationDate"]) : null,
            StockMovementDetailModifierId = dr["stockMovementDetailModifierId"] != DBNull.Value ? Convert.ToInt32(dr["stockMovementDetailModifierId"]) : null,
            StockMovementDetailModificationDate = dr["stockMovementDetailModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockMovementDetailModificationDate"]) : null,
            StockMovementDetailStatusId = Convert.ToBoolean(dr["stockMovementDetailStatusId"])
        };

        public async Task<IEnumerable<StockMovementDetail>> ListarPorMovimientoAsync(int movimientoId)
        {
            var lista = new List<StockMovementDetail>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovementDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailMovementId", movimientoId));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<IEnumerable<StockMovementDetail>> FiltrarAsync(string filtro)
        {
            var lista = new List<StockMovementDetail>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovementDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task NuevoDetalleAsync(StockMovementDetail entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovementDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailMovementId", entity.StockMovementDetailMovementId));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailOrderDetailId", (object?)entity.StockMovementDetailOrderDetailId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailStockId", (object?)entity.StockMovementDetailStockId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailQuantity", entity.StockMovementDetailQuantity));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailFactoryDate", (object?)entity.StockMovementDetailFactoryDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailExpirationDate", (object?)entity.StockMovementDetailExpirationDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailCreatorId", entity.StockMovementDetailCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarDetalleAsync(StockMovementDetail entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovementDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailId", entity.StockMovementDetailId));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailFactoryDate", (object?)entity.StockMovementDetailFactoryDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailExpirationDate", (object?)entity.StockMovementDetailExpirationDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailModifierId", entity.StockMovementDetailModifierId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarDetalleAsync(int id, int modificadorId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_StockMovementDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailId", id));
            cmd.Parameters.Add(new SqlParameter("@stockMovementDetailModifierId", modificadorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}