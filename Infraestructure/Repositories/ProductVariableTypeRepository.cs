using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class ProductVariableTypeRepository : IProductVariableTypeRepository
    {
        private readonly DbConection _conexion;

        public ProductVariableTypeRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private ProductVariableTypes MapearProductVariableType(SqlDataReader dr) => new ProductVariableTypes
        {
            ProductVariableTypeId = Convert.ToInt32(dr["productVariableTypeId"]),
            ProductVariableTypeName = dr["productVariableTypeName"] != DBNull.Value ? dr["productVariableTypeName"].ToString() : null,
            ProductVariableTypeDescription = dr["productVariableTypeDescription"] != DBNull.Value ? dr["productVariableTypeDescription"].ToString() : null,
            ProductVariableTypeCreatorId = Convert.ToInt32(dr["productVariableTypeCreatorId"]),
            ProductVariableTypeCreationDate = dr["productVariableTypeCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productVariableTypeCreationDate"]) : null,
            ProductVariableTypeModificatorId = dr["productVariableTypeModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["productVariableTypeModificatorId"]) : null,
            ProductVariableTypeModificationDate = dr["productVariableTypeModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productVariableTypeModificationDate"]) : null,
            ProductVariableTypeStatusId = Convert.ToBoolean(dr["productVariableTypeStatusId"])
        };

        public async Task<IEnumerable<ProductVariableTypes>> ListarProductVariableTypesAsync()
        {
            var lista = new List<ProductVariableTypes>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductVariableTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearProductVariableType(dr));
            return lista;
        }

        public async Task<IEnumerable<ProductVariableTypes>> ListarProductVariableTypesFiltroAsync(string filtro)
        {
            var lista = new List<ProductVariableTypes>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductVariableTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearProductVariableType(dr));
            return lista;
        }

        public async Task NuevoProductVariableTypeAsync(ProductVariableTypes oProductVariableType)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductVariableTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@productVariableTypeName", oProductVariableType.ProductVariableTypeName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productVariableTypeDescription", oProductVariableType.ProductVariableTypeDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productVariableTypeCreatorId", oProductVariableType.ProductVariableTypeCreatorId));
            var oMsgIns = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumIns = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgIns);
            cmd.Parameters.Add(oNumIns);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumIns.Value) == -1) throw new Exception(oMsgIns.Value?.ToString());
        }

        public async Task EditarProductVariableTypeAsync(ProductVariableTypes oProductVariableType)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductVariableTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@productVariableTypeId", oProductVariableType.ProductVariableTypeId));
            cmd.Parameters.Add(new SqlParameter("@productVariableTypeName", oProductVariableType.ProductVariableTypeName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productVariableTypeDescription", oProductVariableType.ProductVariableTypeDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productVariableTypeModificatorId", oProductVariableType.ProductVariableTypeModificatorId ?? (object)DBNull.Value));
            var oMsgUpd = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumUpd = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgUpd);
            cmd.Parameters.Add(oNumUpd);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumUpd.Value) == -1) throw new Exception(oMsgUpd.Value?.ToString());
        }

        public async Task EliminarProductVariableTypeAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_ProductVariableTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@productVariableTypeId", id));
            cmd.Parameters.Add(new SqlParameter("@productVariableTypeModificatorId", idModificador));
            var oMsgDel = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumDel = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgDel);
            cmd.Parameters.Add(oNumDel);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumDel.Value) == -1) throw new Exception(oMsgDel.Value?.ToString());
        }
    }
}