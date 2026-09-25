using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class ProductIdentificatorRepository : IProductIdentificatorRepository
    {
        private readonly DbConection _conexion;

        public ProductIdentificatorRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private ProductIdentificators MapearProductIdentificator(SqlDataReader dr) => new ProductIdentificators
        {
            ProductIdentificatorId = Convert.ToInt32(dr["productIdentificatorId"]),
            ProductIdentificatorCategoryId = Convert.ToInt32(dr["productIdentificatorCategoryId"]),
            CategoryName = dr["categoryName"] != DBNull.Value ? dr["categoryName"].ToString() : null,
            ProductIdentificatorSubCategoryId = Convert.ToInt32(dr["productIdentificatorSubCategoryId"]),
            SubCategoryName = dr["subCategoryName"] != DBNull.Value ? dr["subCategoryName"].ToString() : null,
            ProductIdentificatorSegmentId = Convert.ToInt32(dr["productIdentificatorSegmentId"]),
            SegmentName = dr["segmentName"] != DBNull.Value ? dr["segmentName"].ToString() : null,
            ProductIdentificatorCreatorId = Convert.ToInt32(dr["productIdentificatorCreatorId"]),
            ProductIdentificatorCreationDate = dr["productIdentificatorCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productIdentificatorCreationDate"]) : null,
            ProductIdentificatorModificatorId = dr["productIdentificatorModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["productIdentificatorModificatorId"]) : null,
            ProductIdentificatorModificationDate = dr["productIdentificatorModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productIdentificatorModificationDate"]) : null,
            ProductIdentificatorStatusId = Convert.ToBoolean(dr["productIdentificatorStatusId"])
        };

        public async Task<IEnumerable<ProductIdentificators>> ListarProductIdentificatorsAsync(bool soloActivos = true)
        {
            var lista = new List<ProductIdentificators>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductIdentificators", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearProductIdentificator(dr));
            return lista;
        }

        public async Task<IEnumerable<ProductIdentificators>> ListarProductIdentificatorsFiltroAsync(string filtro, bool soloActivos = true)
        {
            var lista = new List<ProductIdentificators>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductIdentificators", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearProductIdentificator(dr));
            return lista;
        }

        public async Task NuevoProductIdentificatorAsync(ProductIdentificators oProductIdentificator)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductIdentificators", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorCategoryId", oProductIdentificator.ProductIdentificatorCategoryId));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorSubCategoryId", oProductIdentificator.ProductIdentificatorSubCategoryId));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorSegmentId", oProductIdentificator.ProductIdentificatorSegmentId));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorCreatorId", oProductIdentificator.ProductIdentificatorCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarProductIdentificatorAsync(ProductIdentificators oProductIdentificator)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductIdentificators", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorId", oProductIdentificator.ProductIdentificatorId));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorCategoryId", oProductIdentificator.ProductIdentificatorCategoryId));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorSubCategoryId", oProductIdentificator.ProductIdentificatorSubCategoryId));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorSegmentId", oProductIdentificator.ProductIdentificatorSegmentId));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorModificatorId", oProductIdentificator.ProductIdentificatorModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarProductIdentificatorAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductIdentificators", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorId", id));
            cmd.Parameters.Add(new SqlParameter("@productIdentificatorModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}