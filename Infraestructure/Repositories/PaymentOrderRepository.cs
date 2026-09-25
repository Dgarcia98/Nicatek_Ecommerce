using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class PaymentOrderRepository : IPaymentOrderRepository
    {
        private readonly DbConection _conexion;

        public PaymentOrderRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private PaymentOrder MapearCompleto(SqlDataReader dr) => new PaymentOrder
        {
            OrderId = Convert.ToInt32(dr["orderId"]),
            OrderUserId = Convert.ToInt32(dr["orderUserId"]),
            UserName = dr["userName"] != DBNull.Value ? dr["userName"].ToString() : null,
            UserFullName = dr["userFullName"] != DBNull.Value ? dr["userFullName"].ToString() : null,
            OrderDeliveryAddress = Convert.ToInt32(dr["orderDeliveryAddress"]),
            OrderPaymentMethodId = Convert.ToInt32(dr["orderPaymentMethodId"]),
            OrderSubtotal = Convert.ToDecimal(dr["orderSubtotal"]),
            OrderDiscount = Convert.ToDecimal(dr["orderDiscount"]),
            OrderShipping = Convert.ToDecimal(dr["orderShipping"]),
            OrderTAX = Convert.ToDecimal(dr["orderTAX"]),
            OrderTotal = Convert.ToDecimal(dr["orderTotal"]),
            OrderCurrencyId = Convert.ToInt32(dr["orderCurrencyId"]),
            CurrencyISO = dr["currencyISO"] != DBNull.Value ? dr["currencyISO"].ToString() : null,
            OrderStatusId = Convert.ToInt32(dr["orderStatusId"]),
            StatusName = dr["statusName"] != DBNull.Value ? dr["statusName"].ToString() : null,
            OrderCreatorId = Convert.ToInt32(dr["orderCreatorId"]),
            OrderCreationDate = dr["orderCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["orderCreationDate"]) : null,
            OrderModificatorId = dr["orderModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["orderModificatorId"]) : null,
            OrderModificationDate = dr["orderModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["orderModificationDate"]) : null
        };

        private PaymentOrder MapearResumen(SqlDataReader dr) => new PaymentOrder
        {
            OrderId = Convert.ToInt32(dr["orderId"]),
            OrderUserId = Convert.ToInt32(dr["orderUserId"]),
            UserName = dr["userName"] != DBNull.Value ? dr["userName"].ToString() : null,
            OrderSubtotal = Convert.ToDecimal(dr["orderSubtotal"]),
            OrderDiscount = Convert.ToDecimal(dr["orderDiscount"]),
            OrderShipping = Convert.ToDecimal(dr["orderShipping"]),
            OrderTAX = Convert.ToDecimal(dr["orderTAX"]),
            OrderTotal = Convert.ToDecimal(dr["orderTotal"]),
            OrderCurrencyId = Convert.ToInt32(dr["orderCurrencyId"]),
            CurrencyISO = dr["currencyISO"] != DBNull.Value ? dr["currencyISO"].ToString() : null,
            OrderStatusId = Convert.ToInt32(dr["orderStatusId"]),
            StatusName = dr["statusName"] != DBNull.Value ? dr["statusName"].ToString() : null,
            OrderCreationDate = dr["orderCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["orderCreationDate"]) : null,
            OrderModificationDate = dr["orderModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["orderModificationDate"]) : null
        };

        // El avance de estados se dispara al consultar, no con un trabajo del
        // Agente de SQL Server: así no depende de un servicio que haya que
        // acordarse de arrancar, y en la práctica nadie ve una orden sin haber
        // abierto antes la pantalla que la lista.
        public async Task<int> AvanzarEstadosAsync()
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_AvanzarEstadosOrden", con)
            { CommandType = CommandType.StoredProcedure };
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            var movidas = Convert.ToInt32(oNum.Value);
            return movidas < 0 ? 0 : movidas;
        }

        public async Task<IEnumerable<PaymentOrder>> ListarPorUsuarioAsync(int userId)
        {
            var lista = new List<PaymentOrder>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_PaymentOrders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@orderUserId", userId));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearResumen(dr));
            return lista;
        }

        public async Task<IEnumerable<PaymentOrder>> FiltrarAsync(string? filtro, int? statusId)
        {
            var lista = new List<PaymentOrder>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_PaymentOrders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", (object?)filtro ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@orderStatusId", (object?)statusId ?? DBNull.Value));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearResumen(dr));
            return lista;
        }

        public async Task<PaymentOrder?> ObtenerPorIdAsync(int orderId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_PaymentOrders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "GET"));
            cmd.Parameters.Add(new SqlParameter("@orderId", orderId));
            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync()) return MapearCompleto(dr);
            return null;
        }

        public async Task<int> NuevaOrdenAsync(PaymentOrder entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_PaymentOrders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@orderUserId", entity.OrderUserId));
            cmd.Parameters.Add(new SqlParameter("@orderDeliveryAddress", entity.OrderDeliveryAddress));
            cmd.Parameters.Add(new SqlParameter("@orderPaymentMethodId", entity.OrderPaymentMethodId));
            cmd.Parameters.Add(new SqlParameter("@orderSubtotal", entity.OrderSubtotal));
            cmd.Parameters.Add(new SqlParameter("@orderDiscount", entity.OrderDiscount));
            cmd.Parameters.Add(new SqlParameter("@orderShipping", entity.OrderShipping));
            cmd.Parameters.Add(new SqlParameter("@orderTAX", entity.OrderTAX));
            cmd.Parameters.Add(new SqlParameter("@orderTotal", entity.OrderTotal));
            cmd.Parameters.Add(new SqlParameter("@orderCurrencyId", entity.OrderCurrencyId));
            cmd.Parameters.Add(new SqlParameter("@orderStatusId", entity.OrderStatusId));
            cmd.Parameters.Add(new SqlParameter("@orderCreatorId", entity.OrderCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            var newId = Convert.ToInt32(oNum.Value);
            if (newId == -1) throw new Exception(oMsg.Value?.ToString());
            return newId;
        }

        public async Task<CheckoutResult> CrearOrdenDesdeCarritoAsync(int userId, int addressId, int paymentMethodId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_CrearOrdenDesdeCarrito", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@i_userId", userId));
            cmd.Parameters.Add(new SqlParameter("@i_addressId", addressId));
            cmd.Parameters.Add(new SqlParameter("@i_paymentMethodId", paymentMethodId));

            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            CheckoutResult? resultado = null;
            using (var dr = await cmd.ExecuteReaderAsync())
            {
                if (await dr.ReadAsync())
                {
                    resultado = new CheckoutResult
                    {
                        OrderId = Convert.ToInt32(dr["orderId"]),
                        OrderTotal = Convert.ToDecimal(dr["orderTotal"]),
                        TotalItems = Convert.ToInt32(dr["totalItems"])
                    };
                }
            }

            // Los parametros de salida solo estan disponibles despues de cerrar
            // el reader: leerlos antes devuelve null.
            var codigo = oNum.Value != DBNull.Value ? Convert.ToInt32(oNum.Value) : 500;
            if (codigo != 200 || resultado == null)
                throw new InvalidOperationException(oMsg.Value?.ToString() ?? "No se pudo crear la orden.");

            return resultado;
        }

        public async Task ActualizarEstadoAsync(int orderId, int statusId, int modificatorId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_PaymentOrders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@orderId", orderId));
            cmd.Parameters.Add(new SqlParameter("@orderStatusId", statusId));
            cmd.Parameters.Add(new SqlParameter("@orderModificatorId", modificatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}