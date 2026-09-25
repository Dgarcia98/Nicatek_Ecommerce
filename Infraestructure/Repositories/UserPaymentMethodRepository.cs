using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class UserPaymentMethodRepository : IUserPaymentMethodRepository
    {
        private readonly DbConection _conexion;

        public UserPaymentMethodRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private UserPaymentMethod Mapear(SqlDataReader dr) => new UserPaymentMethod
        {
            UserPaymentMethodId = Convert.ToInt32(dr["userPaymentMethodId"]),
            UserPaymentMethodUserId = Convert.ToInt32(dr["userPaymentMethodUserId"]),
            UserName = dr["userName"] != DBNull.Value ? dr["userName"].ToString() : null,
            UserPaymentMethodPaymentMethodTypeId = Convert.ToInt32(dr["userPaymentMethodPaymentMethodTypeId"]),
            PaymentMethodTypeName = dr["paymentMethodTypeName"] != DBNull.Value ? dr["paymentMethodTypeName"].ToString() : null,
            UserPaymentMethodCardNumber = dr["userPaymentMethodCardNumber"] != DBNull.Value ? (byte[])dr["userPaymentMethodCardNumber"] : null,
            UserPaymentMethodExpirationDate = dr["userPaymentMethodExpirationDate"] != DBNull.Value ? (byte[])dr["userPaymentMethodExpirationDate"] : null,
            UserPaymentMethodCVV = dr["userPaymentMethodCVV"] != DBNull.Value ? (byte[])dr["userPaymentMethodCVV"] : null,
            UserPaymentMethodCardHolderName = dr["userPaymentMethodCardHolderName"] != DBNull.Value ? dr["userPaymentMethodCardHolderName"].ToString() : null,
            UserPaymentMethodCreatorId = Convert.ToInt32(dr["userPaymentMethodCreatorId"]),
            UserPaymentMethodCreationDate = dr["userPaymentMethodCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["userPaymentMethodCreationDate"]) : null,
            UserPaymentMethodModificatorId = dr["userPaymentMethodModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["userPaymentMethodModificatorId"]) : null,
            UserPaymentMethodModificationDate = dr["userPaymentMethodModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["userPaymentMethodModificationDate"]) : null,
            UserPaymentMethodStatusId = Convert.ToBoolean(dr["userPaymentMethodStatusId"])
        };

        public async Task<IEnumerable<UserPaymentMethod>> ListarPorUsuarioAsync(int userId)
        {
            var lista = new List<UserPaymentMethod>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_UserPaymentMethods", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodUserId", userId));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task NuevoMetodoPagoAsync(UserPaymentMethod entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_UserPaymentMethods", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodUserId", entity.UserPaymentMethodUserId));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodPaymentMethodTypeId", entity.UserPaymentMethodPaymentMethodTypeId));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodCardNumber", SqlDbType.VarBinary, 256) { Value = entity.UserPaymentMethodCardNumber ?? (object)DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodExpirationDate", SqlDbType.VarBinary, 256) { Value = entity.UserPaymentMethodExpirationDate ?? (object)DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodCVV", SqlDbType.VarBinary, 256) { Value = entity.UserPaymentMethodCVV ?? (object)DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodCardHolderName", entity.UserPaymentMethodCardHolderName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodCreatorId", entity.UserPaymentMethodCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarMetodoPagoAsync(UserPaymentMethod entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_UserPaymentMethods", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodId", entity.UserPaymentMethodId));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodPaymentMethodTypeId", entity.UserPaymentMethodPaymentMethodTypeId));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodExpirationDate", SqlDbType.VarBinary, 256) { Value = entity.UserPaymentMethodExpirationDate ?? (object)DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodCardHolderName", entity.UserPaymentMethodCardHolderName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodModificatorId", entity.UserPaymentMethodModificatorId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarMetodoPagoAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_UserPaymentMethods", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodId", id));
            cmd.Parameters.Add(new SqlParameter("@userPaymentMethodModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}