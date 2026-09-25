using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly DbConection _conexion;

        public StockRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Stock MapearStock(SqlDataReader dr) => new Stock
        {
            StockId = Convert.ToInt32(dr["stockId"]),
            StockProductVariableId = Convert.ToInt32(dr["stockProductVariableId"]),
            ProductVariableValue = dr["productVariableValue"] != DBNull.Value ? dr["productVariableValue"].ToString() : null,
            ProductId = Convert.ToInt32(dr["productId"]),
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            MarkName = dr["markName"] != DBNull.Value ? dr["markName"].ToString() : null,
            ProviderName = dr["providerName"] != DBNull.Value ? dr["providerName"].ToString() : null,
            StockQuantity = Convert.ToInt32(dr["stockQuantity"]),
            StockFactoryDate = Convert.ToDateTime(dr["stockFactoryDate"]),
            StockExpirationDate = Convert.ToDateTime(dr["stockExpirationDate"]),
            StockProximoVencer = Convert.ToBoolean(dr["stockProximoVencer"]),
            StockVencido = Convert.ToBoolean(dr["stockVencido"]),
            StockCreatorId = Convert.ToInt32(dr["stockCreatorId"]),
            StockCreationDate = dr["stockCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockCreationDate"]) : null,
            StockModificatorId = dr["stockModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["stockModificatorId"]) : null,
            StockModificationDate = dr["stockModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockModificationDate"]) : null,
            StockStatusId = Convert.ToBoolean(dr["stockStatusId"])
        };

        private StockResumen MapearResumen(SqlDataReader dr) => new StockResumen
        {
            ProductId = Convert.ToInt32(dr["productId"]),
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            ProductVariableId = Convert.ToInt32(dr["productVariableId"]),
            ProductVariableValue = dr["productVariableValue"] != DBNull.Value ? dr["productVariableValue"].ToString() : null,
            CurrencyISO = dr["currencyISO"] != DBNull.Value ? dr["currencyISO"].ToString() : null,
            ProductVariablePrice = Convert.ToDecimal(dr["productVariablePrice"]),
            StockTotal = Convert.ToInt32(dr["stockTotal"]),
            StockVencido = Convert.ToInt32(dr["stockVencido"]),
            StockPorVencer = Convert.ToInt32(dr["stockPorVencer"]),
            StockVigente = Convert.ToInt32(dr["stockVigente"]),
            ProximoVencimiento = dr["proximoVencimiento"] != DBNull.Value ? Convert.ToDateTime(dr["proximoVencimiento"]) : null
        };

        public async Task<IEnumerable<Stock>> ListarAsync(int? productVariableId = null, bool soloActivos = true)
        {
            var lista = new List<Stock>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Stocks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@stockProductVariableId",
                productVariableId.HasValue ? (object)productVariableId.Value : DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearStock(dr));
            return lista;
        }

        public async Task<IEnumerable<Stock>> FiltrarAsync(string filtro)
        {
            var lista = new List<Stock>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Stocks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearStock(dr));
            return lista;
        }

        public async Task<IEnumerable<StockResumen>> ResumenAsync(int? productVariableId = null)
        {
            var lista = new List<StockResumen>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Stocks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "SUM"));
            cmd.Parameters.Add(new SqlParameter("@stockProductVariableId",
                productVariableId.HasValue ? (object)productVariableId.Value : DBNull.Value));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearResumen(dr));
            return lista;
        }

        public async Task NuevoStockAsync(Stock entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Stocks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@stockProductVariableId", entity.StockProductVariableId));
            cmd.Parameters.Add(new SqlParameter("@stockQuantity", entity.StockQuantity));
            cmd.Parameters.Add(new SqlParameter("@stockFactoryDate", entity.StockFactoryDate));
            cmd.Parameters.Add(new SqlParameter("@stockExpirationDate", entity.StockExpirationDate));
            cmd.Parameters.Add(new SqlParameter("@stockCreatorId", entity.StockCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarStockAsync(Stock entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Stocks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@stockId", entity.StockId));
            cmd.Parameters.Add(new SqlParameter("@stockProductVariableId", entity.StockProductVariableId));
            cmd.Parameters.Add(new SqlParameter("@stockQuantity", entity.StockQuantity));
            cmd.Parameters.Add(new SqlParameter("@stockFactoryDate", entity.StockFactoryDate));
            cmd.Parameters.Add(new SqlParameter("@stockExpirationDate", entity.StockExpirationDate));
            cmd.Parameters.Add(new SqlParameter("@stockModificatorId", entity.StockModificatorId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarStockAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Stocks", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@stockId", id));
            cmd.Parameters.Add(new SqlParameter("@stockModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}