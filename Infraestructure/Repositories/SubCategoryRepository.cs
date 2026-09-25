using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly DbConection _conexion;

        public SubCategoryRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private SubCategories MapearSubCategory(SqlDataReader dr) => new SubCategories
        {
            SubCategoryId = Convert.ToInt32(dr["subCategoryId"]),
            SubCategoryName = dr["subCategoryName"] != DBNull.Value ? dr["subCategoryName"].ToString() : null,
            SubCategoryDescription = dr["subCategoryDescription"] != DBNull.Value ? dr["subCategoryDescription"].ToString() : null,
            SubCategoryCreatorId = Convert.ToInt32(dr["subCategoryCreatorId"]),
            SubCategoryCreationDate = dr["subCategoryCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["subCategoryCreationDate"]) : null,
            SubCategoryModificatorId = dr["subCategoryModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["subCategoryModificatorId"]) : null,
            SubCategoryModificationDate = dr["subCategoryModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["subCategoryModificationDate"]) : null,
            SubCategoryStatusId = Convert.ToBoolean(dr["subCategoryStatusId"])
        };

        public async Task<IEnumerable<SubCategories>> ListarSubCategoriesAsync(bool soloActivos = true)
        {
            var lista = new List<SubCategories>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_SubCategories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearSubCategory(dr));
            return lista;
        }

        public async Task<IEnumerable<SubCategories>> ListarSubCategoriesFiltroAsync(string filtro)
        {
            var lista = new List<SubCategories>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_SubCategories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearSubCategory(dr));
            return lista;
        }

        public async Task NuevaSubCategoryAsync(SubCategories oSubCategory)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_SubCategories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@subCategoryName", oSubCategory.SubCategoryName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@subCategoryDescription", oSubCategory.SubCategoryDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@subCategoryCreatorId", oSubCategory.SubCategoryCreatorId));
            var oMsgIns = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumIns = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgIns);
            cmd.Parameters.Add(oNumIns);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumIns.Value) == -1) throw new Exception(oMsgIns.Value?.ToString());
        }

        public async Task EditarSubCategoryAsync(SubCategories oSubCategory)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_SubCategories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@subCategoryId", oSubCategory.SubCategoryId));
            cmd.Parameters.Add(new SqlParameter("@subCategoryName", oSubCategory.SubCategoryName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@subCategoryDescription", oSubCategory.SubCategoryDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@subCategoryModificatorId", oSubCategory.SubCategoryModificatorId ?? (object)DBNull.Value));
            var oMsgUpd = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumUpd = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgUpd);
            cmd.Parameters.Add(oNumUpd);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumUpd.Value) == -1) throw new Exception(oMsgUpd.Value?.ToString());
        }

        public async Task EliminarSubCategoryAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_SubCategories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@subCategoryId", id));
            cmd.Parameters.Add(new SqlParameter("@subCategoryModificatorId", idModificador));
            var oMsgDel = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumDel = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgDel);
            cmd.Parameters.Add(oNumDel);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumDel.Value) == -1) throw new Exception(oMsgDel.Value?.ToString());
        }
    }
}