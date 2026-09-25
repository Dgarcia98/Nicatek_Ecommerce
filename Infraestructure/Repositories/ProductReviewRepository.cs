using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly DbConection _conexion;
        public ProductReviewRepository(DbConection conexion) => _conexion = conexion;

        private const string SP = "[SQM_GENERAL].[sp_Tbl_ProductReviews]";

        private static SqlCommand CrearComando(SqlConnection conn, string modo)
        {
            var cmd = new SqlCommand(SP, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", modo);
            cmd.Parameters.Add("@O_Msg", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", SqlDbType.Int).Direction          = ParameterDirection.Output;
            return cmd;
        }

        // El SP valida las reglas de negocio y responde por @O_Num/@O_Msg;
        // se propagan tal cual para que el controlador dé un mensaje útil.
        private static (int codigo, string mensaje) LeerSalida(SqlCommand cmd)
        {
            var num = cmd.Parameters["@O_Num"].Value;
            var msg = cmd.Parameters["@O_Msg"].Value;
            return (
                num != DBNull.Value ? Convert.ToInt32(num) : -1,
                msg != DBNull.Value ? Convert.ToString(msg) ?? "" : ""
            );
        }

        private static ProductReview Mapear(SqlDataReader dr) => new ProductReview
        {
            ReviewId               = Convert.ToInt32(dr["reviewId"]),
            ReviewProductId        = Convert.ToInt32(dr["reviewProductId"]),
            ReviewUserId           = Convert.ToInt32(dr["reviewUserId"]),
            ReviewUserName         = dr["reviewUserName"] != DBNull.Value ? Convert.ToString(dr["reviewUserName"]) : null,
            ReviewRating           = Convert.ToByte(dr["reviewRating"]),
            ReviewComment          = dr["reviewComment"] != DBNull.Value ? Convert.ToString(dr["reviewComment"]) : null,
            ReviewCreationDate     = dr["reviewCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["reviewCreationDate"]) : (DateTime?)null,
            ReviewModificationDate = dr["reviewModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["reviewModificationDate"]) : (DateTime?)null,
            ReviewStatusId         = Convert.ToBoolean(dr["reviewStatusId"]),
        };

        public async Task<IEnumerable<ProductReview>> ListarPorProductoAsync(int productId)
        {
            var lista = new List<ProductReview>();
            using var conn = _conexion.CreateConection();
            using var cmd = CrearComando(conn, "LST");
            cmd.Parameters.AddWithValue("@reviewProductId", productId);
            await conn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<ProductReviewSummary> ObtenerResumenAsync(int productId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = CrearComando(conn, "RES");
            cmd.Parameters.AddWithValue("@reviewProductId", productId);
            await conn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            if (!await dr.ReadAsync()) return new ProductReviewSummary();

            return new ProductReviewSummary
            {
                AverageRating = Convert.ToDecimal(dr["averageRating"]),
                TotalReviews  = Convert.ToInt32(dr["totalReviews"]),
                Count5        = Convert.ToInt32(dr["count5"]),
                Count4        = Convert.ToInt32(dr["count4"]),
                Count3        = Convert.ToInt32(dr["count3"]),
                Count2        = Convert.ToInt32(dr["count2"]),
                Count1        = Convert.ToInt32(dr["count1"]),
            };
        }

        public async Task<ProductReview?> ObtenerDeUsuarioAsync(int productId, int userId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = CrearComando(conn, "GET");
            cmd.Parameters.AddWithValue("@reviewProductId", productId);
            cmd.Parameters.AddWithValue("@reviewUserId", userId);
            await conn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            return await dr.ReadAsync() ? Mapear(dr) : null;
        }

        public async Task<ReviewEligibility> VerificarElegibilidadAsync(int productId, int userId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = CrearComando(conn, "CAN");
            cmd.Parameters.AddWithValue("@reviewProductId", productId);
            cmd.Parameters.AddWithValue("@reviewUserId", userId);
            await conn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            if (!await dr.ReadAsync()) return new ReviewEligibility();

            return new ReviewEligibility
            {
                HasPurchased = Convert.ToBoolean(dr["hasPurchased"]),
                HasReviewed  = Convert.ToBoolean(dr["hasReviewed"]),
            };
        }

        public async Task<(int codigo, string mensaje)> CrearAsync(int productId, int userId, byte rating, string? comentario)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = CrearComando(conn, "INS");
            cmd.Parameters.AddWithValue("@reviewProductId", productId);
            cmd.Parameters.AddWithValue("@reviewUserId", userId);
            cmd.Parameters.AddWithValue("@reviewRating", rating);
            cmd.Parameters.AddWithValue("@reviewComment", (object?)comentario ?? DBNull.Value);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return LeerSalida(cmd);
        }

        public async Task<(int codigo, string mensaje)> ActualizarAsync(int reviewId, int userId, byte? rating, string? comentario)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = CrearComando(conn, "UPD");
            cmd.Parameters.AddWithValue("@reviewId", reviewId);
            cmd.Parameters.AddWithValue("@reviewUserId", userId);
            cmd.Parameters.AddWithValue("@reviewRating", (object?)rating ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@reviewComment", (object?)comentario ?? DBNull.Value);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return LeerSalida(cmd);
        }

        public async Task<(int codigo, string mensaje)> EliminarAsync(int reviewId, int? userId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = CrearComando(conn, "DEL");
            cmd.Parameters.AddWithValue("@reviewId", reviewId);
            // Sin userId el SP no valida pertenencia: es la vía de moderación del admin.
            cmd.Parameters.AddWithValue("@reviewUserId", (object?)userId ?? DBNull.Value);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return LeerSalida(cmd);
        }
    }
}
