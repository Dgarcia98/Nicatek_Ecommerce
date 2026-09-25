using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class PaymentMethodTypeRepository : IPaymentMethodTypeRepository
    {
        private readonly DbConection _conexion;

        public PaymentMethodTypeRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private PaymentMethodTypes MapearPaymentMethodType(SqlDataReader dr) => new PaymentMethodTypes
        {
            PaymentMethodTypeId = Convert.ToInt32(dr["paymentMethodTypeId"]),
            PaymentMethodTypeName = dr["paymentMethodTypeName"] != DBNull.Value ? dr["paymentMethodTypeName"].ToString() : null,
            PaymentMethodTypeDescription = dr["paymentMethodTypeDescription"] != DBNull.Value ? dr["paymentMethodTypeDescription"].ToString() : null,
            PaymentMethodTypeCreatorId = Convert.ToInt32(dr["paymentMethodTypeCreatorId"]),
            PaymentMethodTypeCreationDate = dr["paymentMethodTypeCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["paymentMethodTypeCreationDate"]) : null,
            PaymentMethodTypeModificatorId = dr["paymentMethodTypeModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["paymentMethodTypeModificatorId"]) : null,
            PaymentMethodTypeModificationDate = dr["paymentMethodTypeModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["paymentMethodTypeModificationDate"]) : null,
            PaymentMethodTypeStatusId = Convert.ToBoolean(dr["paymentMethodTypeStatusId"])
        };

        public async Task<IEnumerable<PaymentMethodTypes>> ListarPaymentMethodTypesAsync()
        {
            var lista = new List<PaymentMethodTypes>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_PaymentMethodTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearPaymentMethodType(dr));
            return lista;
        }

        public async Task<IEnumerable<PaymentMethodTypes>> ListarPaymentMethodTypesFiltroAsync(string filtro)
        {
            var lista = new List<PaymentMethodTypes>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_PaymentMethodTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearPaymentMethodType(dr));
            return lista;
        }

        public async Task NuevoPaymentMethodTypeAsync(PaymentMethodTypes oPaymentMethodType)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_PaymentMethodTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeName", oPaymentMethodType.PaymentMethodTypeName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeDescription", oPaymentMethodType.PaymentMethodTypeDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeCreatorId", oPaymentMethodType.PaymentMethodTypeCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarPaymentMethodTypeAsync(PaymentMethodTypes oPaymentMethodType)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_PaymentMethodTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeId", oPaymentMethodType.PaymentMethodTypeId));
            cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeName", oPaymentMethodType.PaymentMethodTypeName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeDescription", oPaymentMethodType.PaymentMethodTypeDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeModificatorId", oPaymentMethodType.PaymentMethodTypeModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarPaymentMethodTypeAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_PaymentMethodTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeId", id));
            cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}