using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class AttributeProductVariableRepository : IAttributeProductVariableRepository
    {
        private readonly DbConection _conexion;

        public AttributeProductVariableRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private AttributeProductVariable Mapear(SqlDataReader dr) => new AttributeProductVariable
        {
            AttributeProductVariableId = Convert.ToInt32(dr["attributeProductVariableId"]),
            AttributeProductVariableProductVariableId = Convert.ToInt32(dr["attributeProductVariableProductVariableId"]),
            ProductVariableValue = dr["productVariableValue"] != DBNull.Value ? dr["productVariableValue"].ToString() : null,
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            AttributeProductVariableAttributeProductId = Convert.ToInt32(dr["attributeProductVariableAttributeProductId"]),
            ProductVariableTypeName = dr["productVariableTypeName"] != DBNull.Value ? dr["productVariableTypeName"].ToString() : null,
            AttributeProductVariableValue = dr["attributeProductVariableValue"] != DBNull.Value ? dr["attributeProductVariableValue"].ToString() : null,
            AttributeProductVariableCreatorId = Convert.ToInt32(dr["attributeProductVariableCreatorId"]),
            AttributeProductVariableCreationDate = dr["attributeProductVariableCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["attributeProductVariableCreationDate"]) : null,
            AttributeProductVariableModificatorId = dr["attributeProductVariableModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["attributeProductVariableModificatorId"]) : null,
            AttributeProductVariableModificationDate = dr["attributeProductVariableModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["attributeProductVariableModificationDate"]) : null,
            AttributeProductVariableStatusId = Convert.ToBoolean(dr["attributeProductVariableStatusId"])
        };

        public async Task<IEnumerable<AttributeProductVariable>> ListarAsync(int? productVariableId = null)
        {
            var lista = new List<AttributeProductVariable>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_AttributeProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableProductVariableId",
                productVariableId.HasValue ? (object)productVariableId.Value : DBNull.Value));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<IEnumerable<AttributeProductVariable>> FiltrarAsync(string filtro)
        {
            var lista = new List<AttributeProductVariable>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_AttributeProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task NuevoAtributoVariableAsync(AttributeProductVariable entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_AttributeProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableProductVariableId", entity.AttributeProductVariableProductVariableId));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableAttributeProductId", entity.AttributeProductVariableAttributeProductId));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableValue", entity.AttributeProductVariableValue ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableCreatorId", entity.AttributeProductVariableCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarAtributoVariableAsync(AttributeProductVariable entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_AttributeProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableId", entity.AttributeProductVariableId));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableProductVariableId", entity.AttributeProductVariableProductVariableId));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableAttributeProductId", entity.AttributeProductVariableAttributeProductId));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableValue", entity.AttributeProductVariableValue ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableModificatorId", entity.AttributeProductVariableModificatorId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarAtributoVariableAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_AttributeProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableId", id));
            cmd.Parameters.Add(new SqlParameter("@attributeProductVariableModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}