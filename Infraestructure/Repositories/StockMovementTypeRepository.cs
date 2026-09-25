using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class StockMovementTypeRepository : IStockMovementTypeRepository
    {
        private readonly DbConection _conexion;

        public StockMovementTypeRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private StockMovementTypes MapearStockMovementType(SqlDataReader dr) => new StockMovementTypes
        {
            StockMovementTypeId = Convert.ToInt32(dr["stockMovementTypeId"]),
            StockMovementTypeName = dr["stockMovementTypeName"] != DBNull.Value ? dr["stockMovementTypeName"].ToString() : null,
            StockMovementTypeDescription = dr["stockMovementTypeDescription"] != DBNull.Value ? dr["stockMovementTypeDescription"].ToString() : null,
            StockMovementTypeCreatorId = Convert.ToInt32(dr["stockMovementTypeCreatorId"]),
            StockMovementTypeCreationDate = dr["stockMovementTypeCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockMovementTypeCreationDate"]) : null,
            StockMovementTypeModificatorId = dr["stockMovementTypeModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["stockMovementTypeModificatorId"]) : null,
            StockMovementTypeModificationDate = dr["stockMovementTypeModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["stockMovementTypeModificationDate"]) : null,
            StockMovementTypeStatusId = Convert.ToBoolean(dr["stockMovementTypeStatusId"])
        };

        public async Task<IEnumerable<StockMovementTypes>> ListarStockMovementTypesAsync()
        {
            var lista = new List<StockMovementTypes>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_StockMovementTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearStockMovementType(dr));
            return lista;
        }

        public async Task<IEnumerable<StockMovementTypes>> ListarStockMovementTypesFiltroAsync(string filtro)
        {
            var lista = new List<StockMovementTypes>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_StockMovementTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearStockMovementType(dr));
            return lista;
        }

        public async Task NuevaStockMovementTypeAsync(StockMovementTypes oStockMovementType)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_StockMovementTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementTypeName", oStockMovementType.StockMovementTypeName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementTypeDescription", oStockMovementType.StockMovementTypeDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementTypeCreatorId", oStockMovementType.StockMovementTypeCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarStockMovementTypeAsync(StockMovementTypes oStockMovementType)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_StockMovementTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementTypeId", oStockMovementType.StockMovementTypeId));
            cmd.Parameters.Add(new SqlParameter("@stockMovementTypeName", oStockMovementType.StockMovementTypeName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementTypeDescription", oStockMovementType.StockMovementTypeDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@stockMovementTypeModificatorId", oStockMovementType.StockMovementTypeModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarStockMovementTypeAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_StockMovementTypes", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@stockMovementTypeId", id));
            cmd.Parameters.Add(new SqlParameter("@stockMovementTypeModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}