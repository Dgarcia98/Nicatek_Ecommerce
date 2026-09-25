using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly DbConection _conexion;

        public UserAddressRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private UserAddress Mapear(SqlDataReader dr) => new UserAddress
        {
            UserAddressId = Convert.ToInt32(dr["userAddressId"]),
            UserAddressUserId = Convert.ToInt32(dr["userAddressUserId"]),
            UserName = dr["userName"] != DBNull.Value ? dr["userName"].ToString() : null,
            UserAddressCountryId = Convert.ToInt32(dr["userAddressCountryId"]),
            UserAddressZIPCode = Convert.ToInt32(dr["userAddressZIPCode"]),
            UserAddressDescription = dr["userAddressDescription"] != DBNull.Value ? dr["userAddressDescription"].ToString() : null,
            UserAddressIsPrincipal = Convert.ToBoolean(dr["userAddressIsPrincipal"]),
            UserAddressCreatorId = Convert.ToInt32(dr["userAddressCreatorId"]),
            UserAddressCreationDate = dr["userAddressCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["userAddressCreationDate"]) : null,
            UserAddressModificatorId = dr["userAddressModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["userAddressModificatorId"]) : null,
            UserAddressModificationDate = dr["userAddressModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["userAddressModificationDate"]) : null,
            UserAddressStatusId = Convert.ToBoolean(dr["userAddressStatusId"])
        };

        public async Task<IEnumerable<UserAddress>> ListarPorUsuarioAsync(int userId)
        {
            var lista = new List<UserAddress>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_UserAddress", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@userAddressUserId", userId));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task NuevadireccionAsync(UserAddress entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_UserAddress", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@userAddressUserId", entity.UserAddressUserId));
            cmd.Parameters.Add(new SqlParameter("@userAddressCountryId", entity.UserAddressCountryId));
            cmd.Parameters.Add(new SqlParameter("@userAddressZIPCode", entity.UserAddressZIPCode));
            cmd.Parameters.Add(new SqlParameter("@userAddressDescription", entity.UserAddressDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@userAddressIsPrincipal", entity.UserAddressIsPrincipal));
            cmd.Parameters.Add(new SqlParameter("@userAddressCreatorId", entity.UserAddressCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarDireccionAsync(UserAddress entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_UserAddress", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@userAddressId", entity.UserAddressId));
            cmd.Parameters.Add(new SqlParameter("@userAddressCountryId", entity.UserAddressCountryId));
            cmd.Parameters.Add(new SqlParameter("@userAddressZIPCode", entity.UserAddressZIPCode));
            cmd.Parameters.Add(new SqlParameter("@userAddressDescription", entity.UserAddressDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@userAddressIsPrincipal", entity.UserAddressIsPrincipal));
            cmd.Parameters.Add(new SqlParameter("@userAddressModificatorId", entity.UserAddressModificatorId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarDireccionAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_UserAddress", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@userAddressId", id));
            cmd.Parameters.Add(new SqlParameter("@userAddressModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}