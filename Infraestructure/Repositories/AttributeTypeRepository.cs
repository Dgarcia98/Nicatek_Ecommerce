using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class AttributeTypeRepository : IAttributeTypeRepository
    {
        private readonly DbConection _conexion;

        public AttributeTypeRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private AttributesTypes MapearAttributeType(SqlDataReader dr) => new AttributesTypes
        {
            AttributeTypeId = Convert.ToInt32(dr["attributeTypeId"]),
            AttributeTypeName = dr["attributeTypeName"] != DBNull.Value ? dr["attributeTypeName"].ToString() : null,
            AttributeTypeDescription = dr["attributeTypeDescription"] != DBNull.Value ? dr["attributeTypeDescription"].ToString() : null,
            AttributeTypeCreatorId = Convert.ToInt32(dr["attributeTypeCreatorId"]),
            AttributeTypeCreationDate = dr["attributeTypeCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["attributeTypeCreationDate"]) : null,
            AttributeTypeModificatorId = dr["attributeTypeModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["attributeTypeModificatorId"]) : null,
            AttributeTypeModificationDate = dr["attributeTypeModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["attributeTypeModificationDate"]) : null,
            AttributeTypeStatusId = Convert.ToBoolean(dr["attributeTypeStatusId"])
        };

        public async Task<IEnumerable<AttributesTypes>> ListarAttributesTypesAsync()
        {
            var lista = new List<AttributesTypes>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributesTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearAttributeType(dr));
            return lista;
        }

        public async Task<IEnumerable<AttributesTypes>> ListarAttributesTypesFiltroAsync(string filtro)
        {
            var lista = new List<AttributesTypes>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributesTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearAttributeType(dr));
            return lista;
        }

        public async Task NuevoAttributeTypeAsync(AttributesTypes oAttributeType)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributesTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@attributeTypeName", oAttributeType.AttributeTypeName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@attributeTypeDescription", oAttributeType.AttributeTypeDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@attributeTypeCreatorId", oAttributeType.AttributeTypeCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarAttributeTypeAsync(AttributesTypes oAttributeType)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributesTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@attributeTypeId", oAttributeType.AttributeTypeId));
            cmd.Parameters.Add(new SqlParameter("@attributeTypeName", oAttributeType.AttributeTypeName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@attributeTypeDescription", oAttributeType.AttributeTypeDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@attributeTypeModificatorId", oAttributeType.AttributeTypeModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAttributeTypeAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_AttributesTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@attributeTypeId", id));
            cmd.Parameters.Add(new SqlParameter("@attributeTypeModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}