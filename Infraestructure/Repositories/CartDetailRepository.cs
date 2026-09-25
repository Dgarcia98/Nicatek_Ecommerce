using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class CartDetailRepository : ICartDetailRepository
    {
        private readonly DbConection _conexion;

        public CartDetailRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private CartDetail Mapear(SqlDataReader dr) => new CartDetail
        {
            CartDetailId = Convert.ToInt32(dr["cartDetailId"]),
            CartDetailCartId = Convert.ToInt32(dr["cartDetailCartId"]),
            CartDetailProductVariableId = Convert.ToInt32(dr["cartDetailProductVariableId"]),
            ProductVariableValue = dr["productVariableValue"] != DBNull.Value ? dr["productVariableValue"].ToString() : null,
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            CartDetailPrice = Convert.ToDecimal(dr["cartDetailPrice"]),
            CartDetailQuantity = Convert.ToInt32(dr["cartDetailQuantity"]),
            CartDetailDiscount = Convert.ToDecimal(dr["cartDetailDiscount"]),
            CartDetailSubTotal = Convert.ToDecimal(dr["cartDetailSubTotal"]),
            CartDetailTAX = Convert.ToDecimal(dr["cartDetailTAX"]),
            CartDetailTotal = Convert.ToDecimal(dr["cartDetailTotal"]),
            CartDetailCurrencyId = Convert.ToInt32(dr["cartDetailCurrencyId"]),
            CurrencyISO = dr["currencyISO"] != DBNull.Value ? dr["currencyISO"].ToString() : null,
            CartDetailCreatorId = Convert.ToInt32(dr["cartDetailCreatorId"]),
            CartDetailCreationDate = dr["cartDetailCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["cartDetailCreationDate"]) : null,
            CartDetailModificatorId = dr["cartDetailModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["cartDetailModificatorId"]) : null,
            CartDetailModificationDate = dr["cartDetailModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["cartDetailModificationDate"]) : null,
            CartDetailStatusId = Convert.ToBoolean(dr["cartDetailStatusId"])
        };

        public async Task<IEnumerable<CartDetail>> ListarPorCarritoAsync(int cartId)
        {
            var lista = new List<CartDetail>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_CartDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@cartDetailCartId", cartId));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task NuevoDetalleAsync(CartDetail entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_CartDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@cartDetailCartId", entity.CartDetailCartId));
            cmd.Parameters.Add(new SqlParameter("@cartDetailProductVariableId", entity.CartDetailProductVariableId));
            cmd.Parameters.Add(new SqlParameter("@cartDetailPrice", entity.CartDetailPrice));
            cmd.Parameters.Add(new SqlParameter("@cartDetailQuantity", entity.CartDetailQuantity));
            cmd.Parameters.Add(new SqlParameter("@cartDetailDiscount", entity.CartDetailDiscount));
            cmd.Parameters.Add(new SqlParameter("@cartDetailSubTotal", entity.CartDetailSubTotal));
            cmd.Parameters.Add(new SqlParameter("@cartDetailTAX", entity.CartDetailTAX));
            cmd.Parameters.Add(new SqlParameter("@cartDetailTotal", entity.CartDetailTotal));
            cmd.Parameters.Add(new SqlParameter("@cartDetailCurrencyId", entity.CartDetailCurrencyId));
            cmd.Parameters.Add(new SqlParameter("@cartDetailCreatorId", entity.CartDetailCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarDetalleAsync(CartDetail entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_CartDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@cartDetailId", entity.CartDetailId));
            cmd.Parameters.Add(new SqlParameter("@cartDetailQuantity", entity.CartDetailQuantity));
            cmd.Parameters.Add(new SqlParameter("@cartDetailDiscount", entity.CartDetailDiscount));
            cmd.Parameters.Add(new SqlParameter("@cartDetailSubTotal", entity.CartDetailSubTotal));
            cmd.Parameters.Add(new SqlParameter("@cartDetailTAX", entity.CartDetailTAX));
            cmd.Parameters.Add(new SqlParameter("@cartDetailTotal", entity.CartDetailTotal));
            cmd.Parameters.Add(new SqlParameter("@cartDetailModificatorId", entity.CartDetailModificatorId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarDetalleAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_CartDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@cartDetailId", id));
            cmd.Parameters.Add(new SqlParameter("@cartDetailModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}