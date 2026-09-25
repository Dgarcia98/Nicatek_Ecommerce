using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;

namespace Infraestructure.Repositories
{
    public class UserFavoriteRepository : IUserFavoriteRepository
    {
        private readonly DbConection _conexion;
        public UserFavoriteRepository(DbConection conexion) => _conexion = conexion;

        private UserFavorites MapearFavorito(SqlDataReader dr) => new UserFavorites
        {
            FavoriteId           = Convert.ToInt32(dr["favoriteId"]),
            FavoriteUserId       = Convert.ToInt32(dr["favoriteUserId"]),
            FavoriteProductId    = Convert.ToInt32(dr["favoriteProductId"]),
            FavoriteCreationDate = dr["favoriteCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["favoriteCreationDate"]) : (DateTime?)null,
            FavoriteStatusId     = Convert.ToBoolean(dr["favoriteStatusId"]),
        };

        public async Task<IEnumerable<UserFavorites>> ListarFavoritosAsync(int userId)
        {
            var lista = new List<UserFavorites>();
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_UserFavorites]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "LST");
            cmd.Parameters.AddWithValue("@favoriteUserId", userId);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearFavorito(dr));
            return lista;
        }

        public async Task<int?> VerificarFavoritoAsync(int userId, int productId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_UserFavorites]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "CHK");
            cmd.Parameters.AddWithValue("@favoriteUserId", userId);
            cmd.Parameters.AddWithValue("@favoriteProductId", productId);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : (int?)null;
        }

        public async Task<int> AgregarFavoritoAsync(int userId, int productId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_UserFavorites]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "INS");
            cmd.Parameters.AddWithValue("@favoriteUserId", userId);
            cmd.Parameters.AddWithValue("@favoriteProductId", productId);
            var pMsg = cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255);
            pMsg.Direction = System.Data.ParameterDirection.Output;
            var pNum = cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int);
            pNum.Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return pNum.Value != DBNull.Value ? Convert.ToInt32(pNum.Value) : -1;
        }

        public async Task QuitarFavoritoAsync(int favoriteId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_UserFavorites]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "DEL");
            cmd.Parameters.AddWithValue("@favoriteId", favoriteId);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
