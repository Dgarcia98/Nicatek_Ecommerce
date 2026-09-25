using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly DbConection _conexion;

        public ProviderRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Providers MapearProvider(SqlDataReader dr) => new Providers
        {
            ProviderId = Convert.ToInt32(dr["providerId"]),
            ProviderName = dr["providerName"] != DBNull.Value ? dr["providerName"].ToString() : null,
            ProviderDescription = dr["providerDescription"] != DBNull.Value ? dr["providerDescription"].ToString() : null,
            ProviderCreatorId = Convert.ToInt32(dr["providerCreatorId"]),
            ProviderCreationDate = dr["providerCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["providerCreationDate"]) : null,
            ProviderModificatorId = dr["providerModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["providerModificatorId"]) : null,
            ProviderModificationDate = dr["providerModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["providerModificationDate"]) : null,
            ProviderStatusId = Convert.ToBoolean(dr["providerStatusId"])
        };

        public async Task<IEnumerable<Providers>> ListarProvidersAsync(bool soloActivos = true)
        {
            var lista = new List<Providers>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Providers", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearProvider(dr));
            return lista;
        }

        public async Task<IEnumerable<Providers>> ListarProvidersFiltroAsync(string filtro)
        {
            var lista = new List<Providers>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Providers", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearProvider(dr));
            return lista;
        }

        public async Task NuevoProviderAsync(Providers oProvider)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Providers", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@providerName", oProvider.ProviderName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@providerDescription", oProvider.ProviderDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@providerCreatorId", oProvider.ProviderCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarProviderAsync(Providers oProvider)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Providers", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@providerId", oProvider.ProviderId));
            cmd.Parameters.Add(new SqlParameter("@providerName", oProvider.ProviderName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@providerDescription", oProvider.ProviderDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@providerModificatorId", oProvider.ProviderModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarProviderAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Providers", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@providerId", id));
            cmd.Parameters.Add(new SqlParameter("@providerModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}