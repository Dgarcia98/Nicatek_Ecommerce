using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class PaymentOrderDetailRepository : IPaymentOrderDetailRepository
    {
        private readonly DbConection _conexion;

        public PaymentOrderDetailRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private PaymentOrderDetail Mapear(SqlDataReader dr) => new PaymentOrderDetail
        {
            OrderDetailId = Convert.ToInt32(dr["orderDetailId"]),
            OrderDetailOrderId = Convert.ToInt32(dr["orderDetailOrderId"]),
            OrderDetailProductVariableId = Convert.ToInt32(dr["orderDetailProductVariableId"]),
            ProductVariableValue = dr["productVariableValue"] != DBNull.Value ? dr["productVariableValue"].ToString() : null,
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            OrderDetailPrice = Convert.ToDecimal(dr["orderDetailPrice"]),
            OrderDetailQuantity = Convert.ToInt32(dr["orderDetailQuantity"]),
            OrderDetailDiscount = Convert.ToDecimal(dr["orderDetailDiscount"]),
            OrderDetailSubTotal = Convert.ToDecimal(dr["orderDetailSubTotal"]),
            OrderDetailTAX = Convert.ToDecimal(dr["orderDetailTAX"]),
            OrderDetailTotal = Convert.ToDecimal(dr["orderDetailTotal"]),
            OrderDetailCurrencyId = Convert.ToInt32(dr["orderDetailCurrencyId"]),
            CurrencyISO = dr["currencyISO"] != DBNull.Value ? dr["currencyISO"].ToString() : null,
            OrderDetailCreatorId = Convert.ToInt32(dr["orderDetailCreatorId"]),
            OrderDetailCreationDate = dr["orderDetailCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["orderDetailCreationDate"]) : null,
            OrderDetailModificatorId = dr["orderDetailModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["orderDetailModificatorId"]) : null,
            OrderDetailModificationDate = dr["orderDetailModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["orderDetailModificationDate"]) : null,
            OrderDetailStatusId = Convert.ToBoolean(dr["orderDetailStatusId"])
        };

        public async Task<IEnumerable<PaymentOrderDetail>> ListarPorOrdenAsync(int orderId)
        {
            var lista = new List<PaymentOrderDetail>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_PaymentOrderDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@orderDetailOrderId", orderId));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<PaymentOrderDetail?> ObtenerPorIdAsync(int orderDetailId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_PaymentOrderDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "GET"));
            cmd.Parameters.Add(new SqlParameter("@orderDetailId", orderDetailId));
            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync()) return Mapear(dr);
            return null;
        }

        public async Task NuevoDetalleAsync(PaymentOrderDetail entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_PaymentOrderDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@orderDetailOrderId", entity.OrderDetailOrderId));
            cmd.Parameters.Add(new SqlParameter("@orderDetailProductVariableId", entity.OrderDetailProductVariableId));
            cmd.Parameters.Add(new SqlParameter("@orderDetailPrice", entity.OrderDetailPrice));
            cmd.Parameters.Add(new SqlParameter("@orderDetailQuantity", entity.OrderDetailQuantity));
            cmd.Parameters.Add(new SqlParameter("@orderDetailDiscount", entity.OrderDetailDiscount));
            cmd.Parameters.Add(new SqlParameter("@orderDetailSubTotal", entity.OrderDetailSubTotal));
            cmd.Parameters.Add(new SqlParameter("@orderDetailTAX", entity.OrderDetailTAX));
            cmd.Parameters.Add(new SqlParameter("@orderDetailTotal", entity.OrderDetailTotal));
            cmd.Parameters.Add(new SqlParameter("@orderDetailCurrencyId", entity.OrderDetailCurrencyId));
            cmd.Parameters.Add(new SqlParameter("@orderDetailCreatorId", entity.OrderDetailCreatorId));
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
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_PaymentOrderDetails", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@orderDetailId", id));
            cmd.Parameters.Add(new SqlParameter("@orderDetailModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}