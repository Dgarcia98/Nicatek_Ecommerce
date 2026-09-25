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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbConection _conexion;

        public CategoryRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Categories MapearCategory(SqlDataReader dr) => new Categories
        {
            CategoryId = Convert.ToInt32(dr["categoryId"]),
            CategoryName = dr["categoryName"] != DBNull.Value ? dr["categoryName"].ToString() : null,
            CategoryDescription = dr["categoryDescription"] != DBNull.Value ? dr["categoryDescription"].ToString() : null,
            CategoryCreatorId = Convert.ToInt32(dr["categoryCreatorId"]),
            CategoryCreationDate = dr["categoryCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["categoryCreationDate"]) : null,
            CategoryModificatorId = dr["categoryModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["categoryModificatorId"]) : null,
            CategoryModificationDate = dr["categoryModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["categoryModificationDate"]) : null,
            CategoryStatusId = Convert.ToBoolean(dr["categoryStatusId"])
        };

        public async Task<IEnumerable<Categories>> ListarCategoriesAsync(bool soloActivos = true)
        {
            var lista = new List<Categories>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Categories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearCategory(dr));
            return lista;
        }

        public async Task<IEnumerable<Categories>> ListarCategoriesFiltroAsync(string filtro)
        {
            var lista = new List<Categories>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Categories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearCategory(dr));
            return lista;
        }

        public async Task NuevaCategoryAsync(Categories oCategory)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Categories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@categoryName", oCategory.CategoryName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@categoryDescription", oCategory.CategoryDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@categoryCreatorId", oCategory.CategoryCreatorId));
            var oMsgIns = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumIns = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgIns);
            cmd.Parameters.Add(oNumIns);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumIns.Value) == -1) throw new Exception(oMsgIns.Value?.ToString());
        }

        public async Task EditarCategoryAsync(Categories oCategory)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Categories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@categoryId", oCategory.CategoryId));
            cmd.Parameters.Add(new SqlParameter("@categoryName", oCategory.CategoryName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@categoryDescription", oCategory.CategoryDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@categoryModificatorId", oCategory.CategoryModificatorId ?? (object)DBNull.Value));
            var oMsgUpd = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumUpd = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgUpd);
            cmd.Parameters.Add(oNumUpd);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumUpd.Value) == -1) throw new Exception(oMsgUpd.Value?.ToString());
        }

        public async Task EliminarCategoryAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Categories", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@categoryId", id));
            cmd.Parameters.Add(new SqlParameter("@categoryModificatorId", idModificador));
            var oMsgDel = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNumDel = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsgDel);
            cmd.Parameters.Add(oNumDel);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNumDel.Value) == -1) throw new Exception(oMsgDel.Value?.ToString());
        }
    }

}

