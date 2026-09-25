using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly DbConection _conexion;

        public ProductImageRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private ProductImage Mapear(SqlDataReader dr) => new ProductImage
        {
            ProductImageId = Convert.ToInt32(dr["productImageId"]),
            ProductImageProductId = Convert.ToInt32(dr["productImageProductId"]),
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            ProductImageURL = dr["productImageURL"] != DBNull.Value ? dr["productImageURL"].ToString() : null,
            ProductImageDescription = dr["productImageDescription"] != DBNull.Value ? dr["productImageDescription"].ToString() : null,
            ProductImageIsPrincipal = Convert.ToBoolean(dr["productImageIsPrincipal"]),
            ProductImageCreatorId = Convert.ToInt32(dr["productImageCreatorId"]),
            ProductImageCreationDate = dr["productImageCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productImageCreationDate"]) : null,
            ProductImageModificatorId = dr["productImageModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["productImageModificatorId"]) : null,
            ProductImageModificationDate = dr["productImageModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productImageModificationDate"]) : null,
            ProductImageStatusId = Convert.ToBoolean(dr["productImageStatusId"])
        };

        // ── Imágenes con archivo (VARBINARY) ──────────────────────────────
        //
        // Van en procedimientos propios y no como modos de sp_Tbl_ProductImages
        // porque el reparto de datos es distinto: el listado nunca debe traer
        // los bytes, y el archivo suelto nunca debe traer el resto.

        public async Task<IEnumerable<ProductImageInfo>> ImagenesDeProductoAsync(int productId, int? productVariableId)
        {
            var lista = new List<ProductImageInfo>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_ImagenesDeProducto", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@productId", productId));
            cmd.Parameters.Add(new SqlParameter("@productVariableId", (object?)productVariableId ?? DBNull.Value));

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(new ProductImageInfo
                {
                    ProductImageId          = Convert.ToInt32(dr["productImageId"]),
                    ProductImageProductId   = Convert.ToInt32(dr["productImageProductId"]),
                    ProductImageVariableId  = dr["productImageVariableId"] == DBNull.Value ? null : Convert.ToInt32(dr["productImageVariableId"]),
                    ProductImageIsPrincipal = Convert.ToBoolean(dr["productImageIsPrincipal"]),
                    ProductImageOrder       = Convert.ToInt32(dr["productImageOrder"]),
                    ProductImageContentType = dr["productImageContentType"] as string,
                    ProductImageDescription = dr["productImageDescription"] as string,
                    ProductImageURL         = dr["productImageURL"] as string,
                    TieneArchivo            = Convert.ToInt32(dr["tieneArchivo"]) == 1,
                    ProductImageVersion     = dr["productImageVersion"] == DBNull.Value ? 0L : Convert.ToInt64(dr["productImageVersion"]),
                });
            }
            return lista;
        }

        public async Task<ProductImageFile?> ArchivoAsync(int productImageId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_ImagenArchivo", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@productImageId", productImageId));

            // SequentialAccess: el archivo se lee en cuanto llega, sin que ADO
            // guarde la fila entera en memoria antes de entregarla.
            using var dr = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
            if (!await dr.ReadAsync()) return null;

            var bytes = dr["productImageBytes"] as byte[];
            if (bytes is null || bytes.Length == 0) return null;

            return new ProductImageFile
            {
                Bytes = bytes,
                ContentType = dr["productImageContentType"] as string ?? "image/jpeg",
            };
        }

        public async Task<int> GuardarImagenAsync(int productId, int? productVariableId, byte[] bytes,
                                                  string contentType, string? descripcion, bool esPrincipal, int creadorId,
                                                  int? productImageId = null)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_GuardarImagenProducto", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@productImageId", (object?)productImageId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productImageProductId", productId));
            cmd.Parameters.Add(new SqlParameter("@productImageVariableId", (object?)productVariableId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productImageBytes", SqlDbType.VarBinary, -1) { Value = bytes });
            cmd.Parameters.Add(new SqlParameter("@productImageContentType", contentType));
            cmd.Parameters.Add(new SqlParameter("@productImageDescription", (object?)descripcion ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productImageIsPrincipal", esPrincipal));
            cmd.Parameters.Add(new SqlParameter("@productImageCreatorId", creadorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            await cmd.ExecuteNonQueryAsync();
            var num = Convert.ToInt32(oNum.Value);
            if (num <= 0) throw new Exception(oMsg.Value?.ToString() ?? "No se pudo guardar la imagen.");
            return num;
        }

        public async Task<IEnumerable<ProductImage>> ListarAsync(int? productId = null)
        {
            var lista = new List<ProductImage>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductImages", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@productImageProductId",
                productId.HasValue ? (object)productId.Value : DBNull.Value));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<IEnumerable<ProductImage>> FiltrarAsync(string filtro)
        {
            var lista = new List<ProductImage>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductImages", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task NuevaImagenAsync(ProductImage entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductImages", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@productImageProductId", entity.ProductImageProductId));
            cmd.Parameters.Add(new SqlParameter("@productImageURL", entity.ProductImageURL ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productImageDescription", entity.ProductImageDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productImageIsPrincipal", entity.ProductImageIsPrincipal));
            cmd.Parameters.Add(new SqlParameter("@productImageCreatorId", entity.ProductImageCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarImagenAsync(ProductImage entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductImages", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@productImageId", entity.ProductImageId));
            cmd.Parameters.Add(new SqlParameter("@productImageURL", entity.ProductImageURL ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productImageDescription", entity.ProductImageDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productImageIsPrincipal", entity.ProductImageIsPrincipal));
            cmd.Parameters.Add(new SqlParameter("@productImageModificatorId", entity.ProductImageModificatorId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarImagenAsync(int id, int idModificador, bool? nuevoEstado = null)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductImages", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@productImageId", id));
            cmd.Parameters.Add(new SqlParameter("@productImageModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@nuevoEstado", (object?)nuevoEstado ?? DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}