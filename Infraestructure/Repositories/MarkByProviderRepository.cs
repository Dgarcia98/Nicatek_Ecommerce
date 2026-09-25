using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class MarkByProviderRepository : IMarkByProviderRepository
    {
        private readonly DbConection _conexion;

        public MarkByProviderRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private MarkByProviders MapearMarkByProvider(SqlDataReader dr) => new MarkByProviders
        {
            MarkByProviderId = Convert.ToInt32(dr["markByProviderId"]),
            MarkByProviderMarkId = Convert.ToInt32(dr["markByProviderMarkId"]),
            MarkName = dr["markName"] != DBNull.Value ? dr["markName"].ToString() : null,
            MarkByProviderProviderId = Convert.ToInt32(dr["markByProviderProviderId"]),
            ProviderName = dr["providerName"] != DBNull.Value ? dr["providerName"].ToString() : null,
            MarkByProviderCreatorId = Convert.ToInt32(dr["markByProviderCreatorId"]),
            MarkByProviderCreationDate = dr["markByProviderCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["markByProviderCreationDate"]) : null,
            MarkByProviderModificatorId = dr["markByProviderModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["markByProviderModificatorId"]) : null,
            MarkByProviderModificationDate = dr["markByProviderModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["markByProviderModificationDate"]) : null,
            MarkByProviderStatusId = Convert.ToBoolean(dr["markByProviderStatusId"])
        };

        public async Task<IEnumerable<MarkByProviders>> ListarMarkByProvidersAsync()
        {
            var lista = new List<MarkByProviders>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_MarkByProviders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearMarkByProvider(dr));
            return lista;
        }

        public async Task<IEnumerable<MarkByProviders>> ListarMarkByProvidersFiltroAsync(string filtro)
        {
            var lista = new List<MarkByProviders>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_MarkByProviders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearMarkByProvider(dr));
            return lista;
        }

        public async Task NuevoMarkByProviderAsync(MarkByProviders oMarkByProvider)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_MarkByProviders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@markByProviderMarkId", oMarkByProvider.MarkByProviderMarkId));
            cmd.Parameters.Add(new SqlParameter("@markByProviderProviderId", oMarkByProvider.MarkByProviderProviderId));
            cmd.Parameters.Add(new SqlParameter("@markByProviderCreatorId", oMarkByProvider.MarkByProviderCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarMarkByProviderAsync(MarkByProviders oMarkByProvider)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_MarkByProviders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@markByProviderId", oMarkByProvider.MarkByProviderId));
            cmd.Parameters.Add(new SqlParameter("@markByProviderMarkId", oMarkByProvider.MarkByProviderMarkId));
            cmd.Parameters.Add(new SqlParameter("@markByProviderProviderId", oMarkByProvider.MarkByProviderProviderId));
            cmd.Parameters.Add(new SqlParameter("@markByProviderModificatorId", oMarkByProvider.MarkByProviderModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarMarkByProviderAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_MarkByProviders", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@markByProviderId", id));
            cmd.Parameters.Add(new SqlParameter("@markByProviderModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}