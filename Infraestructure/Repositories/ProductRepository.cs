using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// Comprueba que la columna venga en el resultado.
        ///
        /// La vista del home puede estar en su versión anterior mientras no se
        /// vuelva a ejecutar el script: sin esta guarda, la API se cae entera
        /// con "no se encontró la columna" en vez de servir el catálogo sin
        /// imágenes nuevas.
        /// </summary>
        private static bool TieneColumna(SqlDataReader dr, string nombre)
        {
            for (var i = 0; i < dr.FieldCount; i++)
                if (string.Equals(dr.GetName(i), nombre, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private readonly DbConection _conexion;

        public ProductRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Product Mapear(SqlDataReader dr) => new Product
        {
            ProductId = Convert.ToInt32(dr["productId"]),
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            ProductDescription = dr["productDescription"] != DBNull.Value ? dr["productDescription"].ToString() : null,
            ProductProductIdentificatorId = Convert.ToInt32(dr["productProductIdentificatorId"]),
            CategoryName = dr["categoryName"] != DBNull.Value ? dr["categoryName"].ToString() : null,
            SubCategoryName = dr["subCategoryName"] != DBNull.Value ? dr["subCategoryName"].ToString() : null,
            SegmentName = dr["segmentName"] != DBNull.Value ? dr["segmentName"].ToString() : null,
            ProductMarkByProviderId = Convert.ToInt32(dr["productMarkByProviderId"]),
            MarkName = dr["markName"] != DBNull.Value ? dr["markName"].ToString() : null,
            ProviderName = dr["providerName"] != DBNull.Value ? dr["providerName"].ToString() : null,
            ProductCreatorId = Convert.ToInt32(dr["productCreatorId"]),
            ProductCreationDate = dr["productCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productCreationDate"]) : null,
            ProductModificatorId = dr["productModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["productModificatorId"]) : null,
            ProductModificationDate = dr["productModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productModificationDate"]) : null,
            ProductStatusId = Convert.ToBoolean(dr["productStatusId"]),
            ProductImageURL = dr["productImageURL"] != DBNull.Value ? dr["productImageURL"].ToString() : null
        };

        public async Task<IEnumerable<Product>> ListarAsync(bool soloActivos = true)
        {
            var lista = new List<Product>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Products", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<IEnumerable<Product>> FiltrarAsync(string filtro, bool soloActivos = true)
        {
            var lista = new List<Product>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Products", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<IEnumerable<ProductHome>> DestacadosAsync(int top)
        {
            var lista = new List<ProductHome>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_ProductosDestacados", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@i_top", top));

            using var dr = await cmd.ExecuteReaderAsync();
            // El procedimiento devuelve las columnas de Vw_ProductosHome mas dos
            // de calculo, asi que el mapeo del listado sirve tal cual.
            while (await dr.ReadAsync()) lista.Add(MapearHome(dr));
            return lista;
        }

        private ProductHome MapearHome(SqlDataReader dr) => new ProductHome
        {
            ProductId = Convert.ToInt32(dr["productId"]),
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            ProductDescription = dr["productDescription"] != DBNull.Value ? dr["productDescription"].ToString() : null,
            ProductProductIdentificatorId = Convert.ToInt32(dr["productProductIdentificatorId"]),
            CategoryId = Convert.ToInt32(dr["categoryId"]),
            CategoryName = dr["categoryName"] != DBNull.Value ? dr["categoryName"].ToString() : null,
            SubCategoryId = Convert.ToInt32(dr["subCategoryId"]),
            SubCategoryName = dr["subCategoryName"] != DBNull.Value ? dr["subCategoryName"].ToString() : null,
            SegmentId = Convert.ToInt32(dr["segmentId"]),
            SegmentName = dr["segmentName"] != DBNull.Value ? dr["segmentName"].ToString() : null,
            ProductMarkByProviderId = Convert.ToInt32(dr["productMarkByProviderId"]),
            MarkId = Convert.ToInt32(dr["markId"]),
            MarkName = dr["markName"] != DBNull.Value ? dr["markName"].ToString() : null,
            ProviderId = Convert.ToInt32(dr["providerId"]),
            ProviderName = dr["providerName"] != DBNull.Value ? dr["providerName"].ToString() : null,
            ProductStatusId = Convert.ToBoolean(dr["productStatusId"]),
            ProductImageURL = dr["productImageURL"] != DBNull.Value ? dr["productImageURL"].ToString() : null,
            ProductImageId  = TieneColumna(dr, "productImageId") && dr["productImageId"] != DBNull.Value ? Convert.ToInt32(dr["productImageId"]) : null,
            ProductImageVersion = TieneColumna(dr, "productImageVersion") && dr["productImageVersion"] != DBNull.Value ? Convert.ToInt64(dr["productImageVersion"]) : 0L,
            EsNuevo = TieneColumna(dr, "esNuevo") && dr["esNuevo"] != DBNull.Value && Convert.ToBoolean(dr["esNuevo"]),
            PrecioLista = TieneColumna(dr, "precioLista") && dr["precioLista"] != DBNull.Value ? Convert.ToDecimal(dr["precioLista"]) : null,
            Descuento = TieneColumna(dr, "descuento") && dr["descuento"] != DBNull.Value ? Convert.ToDecimal(dr["descuento"]) : 0m,
            MinPrice = Convert.ToDecimal(dr["minPrice"]),
            CurrencyId = Convert.ToInt32(dr["currencyId"]),
            CurrencyISO = dr["currencyISO"] != DBNull.Value ? dr["currencyISO"].ToString() : null,
            // Calificación agregada. Se comprueba DBNull porque una vista antigua
            // (sin estas columnas) haría fallar el mapeo de todo el listado.
            // Con la misma guarda que la imagen: hay dos archivos que definen
            // Vw_ProductosHome y el simple no trae estas columnas. Si se ejecuta
            // ese por error, sin la guarda la lectura lanza excepción y el
            // catálogo entero desaparece de la app. Preferimos servir los
            // productos sin valoración a no servir ninguno.
            AvgRating = TieneColumna(dr, "avgRating") && dr["avgRating"] != DBNull.Value ? Convert.ToDecimal(dr["avgRating"]) : 0m,
            TotalReviews = TieneColumna(dr, "totalReviews") && dr["totalReviews"] != DBNull.Value ? Convert.ToInt32(dr["totalReviews"]) : 0
        };

        public async Task<(IEnumerable<ProductHome> Items, int TotalRows)> ListarPaginadoAsync(
            int pageNumber, int pageSize, string? search, int? categoryId,
            string? markNames, string? subCategories, decimal? priceMin, decimal? priceMax, string? sort,
            string? productIds = null, int? seed = null, bool soloOferta = false)
        {
            var lista = new List<ProductHome>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.USP_ProductsHome", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@i_pageNumber", pageNumber));
            cmd.Parameters.Add(new SqlParameter("@i_pageSize", pageSize));
            cmd.Parameters.Add(new SqlParameter("@i_search", (object?)search ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@i_categoryId", (object?)categoryId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@i_markNames", (object?)markNames ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@i_subCategories", (object?)subCategories ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@i_priceMin", (object?)priceMin ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@i_priceMax", (object?)priceMax ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@i_sort", (object?)sort ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@i_seed", (object?)seed ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@i_soloOferta", soloOferta));
            cmd.Parameters.Add(new SqlParameter("@i_productIds", (object?)productIds ?? DBNull.Value));

            var oTotal = new SqlParameter("@o_totalRows", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var oMsg = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oTotal);
            cmd.Parameters.Add(oCode);
            cmd.Parameters.Add(oMsg);

            using (var dr = await cmd.ExecuteReaderAsync())
            {
                while (await dr.ReadAsync()) lista.Add(MapearHome(dr));
            }
            // Los parámetros OUTPUT solo quedan poblados después de cerrar el reader.
            var total = oTotal.Value != DBNull.Value ? Convert.ToInt32(oTotal.Value) : 0;
            return (lista, total);
        }

        public async Task<Product?> ObtenerPorIdAsync(int id)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Products", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "GET"));
            cmd.Parameters.Add(new SqlParameter("@productId", id));
            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync()) return Mapear(dr);
            return null;
        }

        public async Task NuevoProductoAsync(Product entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Products", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@productName", entity.ProductName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productDescription", entity.ProductDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productProductIdentificatorId", entity.ProductProductIdentificatorId));
            cmd.Parameters.Add(new SqlParameter("@productMarkByProviderId", entity.ProductMarkByProviderId));
            cmd.Parameters.Add(new SqlParameter("@productCreatorId", entity.ProductCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarProductoAsync(Product entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Products", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@productId", entity.ProductId));
            cmd.Parameters.Add(new SqlParameter("@productName", entity.ProductName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productDescription", entity.ProductDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productProductIdentificatorId", entity.ProductProductIdentificatorId));
            cmd.Parameters.Add(new SqlParameter("@productMarkByProviderId", entity.ProductMarkByProviderId));
            cmd.Parameters.Add(new SqlParameter("@productModificatorId", entity.ProductModificatorId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarProductoAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_Products", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@productId", id));
            cmd.Parameters.Add(new SqlParameter("@productModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}