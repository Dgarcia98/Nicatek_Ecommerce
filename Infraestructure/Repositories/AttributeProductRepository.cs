using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class AttributeProductRepository : IAttributeProductRepository
    {
        private readonly DbConection _conexion;

        public AttributeProductRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private AttributeProduct Mapear(SqlDataReader dr) => new AttributeProduct
        {
            AttributeProductId = Convert.ToInt32(dr["AttributeProductId"]),
            AttributeProductProductId = Convert.ToInt32(dr["attributeProductProductId"]),
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            AttributeProductAttributesTypeId = Convert.ToInt32(dr["AttributeProductAttributesTypeId"]),
            AttributeTypeName = dr["attributeTypeName"] != DBNull.Value ? dr["attributeTypeName"].ToString() : null,
            AttributeProductName = dr["AttributeProductName"] != DBNull.Value ? dr["AttributeProductName"].ToString() : null,
            AttributeProductDescription = dr["AttributeProductDescription"] != DBNull.Value ? dr["AttributeProductDescription"].ToString() : null,
            AttributeProductCreatorId = Convert.ToInt32(dr["AttributeProductCreatorId"]),
            AttributeProductCreationDate = dr["AttributeProductCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["AttributeProductCreationDate"]) : null,
            AttributeProductModificatorId = dr["AttributeProductModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["AttributeProductModificatorId"]) : null,
            AttributeProductModificationDate = dr["AttributeProductModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["AttributeProductModificationDate"]) : null,
            AttributeProductStatusId = Convert.ToBoolean(dr["AttributeProductStatusId"])
        };

        public async Task<IEnumerable<AttributeProduct>> ListarAsync(int? productId = null)
        {
            var lista = new List<AttributeProduct>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributeProducts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@attributeProductProductId",
                productId.HasValue ? (object)productId.Value : DBNull.Value));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<IEnumerable<AttributeProduct>> FiltrarAsync(string filtro)
        {
            var lista = new List<AttributeProduct>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributeProducts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<AttributeProduct?> ObtenerPorIdAsync(int id)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributeProducts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "GET"));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductId", id));
            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync()) return Mapear(dr);
            return null;
        }

        public async Task NuevoAtributoAsync(AttributeProduct entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributeProducts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@attributeProductProductId", entity.AttributeProductProductId));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductAttributesTypeId", entity.AttributeProductAttributesTypeId));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductName", entity.AttributeProductName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductDescription", entity.AttributeProductDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductCreatorId", entity.AttributeProductCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarAtributoAsync(AttributeProduct entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributeProducts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductId", entity.AttributeProductId));
            cmd.Parameters.Add(new SqlParameter("@attributeProductProductId", entity.AttributeProductProductId));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductAttributesTypeId", entity.AttributeProductAttributesTypeId));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductName", entity.AttributeProductName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductDescription", entity.AttributeProductDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductModificatorId", entity.AttributeProductModificatorId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarAtributoAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributeProducts", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductId", id));
            cmd.Parameters.Add(new SqlParameter("@AttributeProductModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}