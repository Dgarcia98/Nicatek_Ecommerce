using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DbConection _conexion;

        public CartRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Cart Mapear(SqlDataReader dr) => new Cart
        {
            CartId = Convert.ToInt32(dr["cartId"]),
            CartUserId = Convert.ToInt32(dr["cartUserId"]),
            UserName = dr["userName"] != DBNull.Value ? dr["userName"].ToString() : null,
            UserFullName = dr["userFullName"] != DBNull.Value ? dr["userFullName"].ToString() : null,
            CartCreatorId = Convert.ToInt32(dr["cartCreatorId"]),
            CartCreationDate = dr["cartCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["cartCreationDate"]) : null,
            CartModificatorId = dr["cartModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["cartModificatorId"]) : null,
            CartModificationDate = dr["cartModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["cartModificationDate"]) : null,
            CartStatusId = Convert.ToBoolean(dr["cartStatusId"])
        };

        public async Task<IEnumerable<Cart>> ListarPorUsuarioAsync(int userId)
        {
            var lista = new List<Cart>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Carts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@cartUserId", userId));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<Cart?> ObtenerPorIdAsync(int cartId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Carts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "GET"));
            cmd.Parameters.Add(new SqlParameter("@cartId", cartId));
            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync()) return Mapear(dr);
            return null;
        }

        public async Task NuevoCarritoAsync(Cart entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Carts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@cartUserId", entity.CartUserId));
            cmd.Parameters.Add(new SqlParameter("@cartCreatorId", entity.CartCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarCarritoAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Carts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@cartId", id));
            cmd.Parameters.Add(new SqlParameter("@cartModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}