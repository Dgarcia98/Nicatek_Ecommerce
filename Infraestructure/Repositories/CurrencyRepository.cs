using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly DbConection _conexion;

        public CurrencyRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        private Currencies MapearCurrency(SqlDataReader dr) => new Currencies
        {
            CurrencyId = Convert.ToInt32(dr["currencyId"]),
            CurrencyName = dr["currencyName"] != DBNull.Value ? dr["currencyName"].ToString() : null,
            CurrencyISO = dr["currencyISO"] != DBNull.Value ? dr["currencyISO"].ToString() : null,
            CurrencyCode = Convert.ToInt32(dr["currencyCode"]),
            CurrencyDescription = dr["currencyDescription"] != DBNull.Value ? dr["currencyDescription"].ToString() : null,
            CurrencyCreatorId = Convert.ToInt32(dr["currencyCreatorId"]),
            CurrencyCreationDate = dr["currencyCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["currencyCreationDate"]) : null,
            CurrencyModificatorId = dr["currencyModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["currencyModificatorId"]) : null,
            CurrencyModificationDate = dr["currencyModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["currencyModificationDate"]) : null,
            CurrencyStatusId = Convert.ToBoolean(dr["currencyStatusId"])
        };

        public async Task<IEnumerable<Currencies>> ListarCurrenciesAsync()
        {
            var lista = new List<Currencies>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Currencies", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearCurrency(dr));
            return lista;
        }

        public async Task<IEnumerable<Currencies>> ListarCurrenciesFiltroAsync(string filtro)
        {
            var lista = new List<Currencies>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Currencies", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearCurrency(dr));
            return lista;
        }

        public async Task NuevoCurrencyAsync(Currencies oCurrency)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Currencies", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@currencyName", oCurrency.CurrencyName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@currencyISO", oCurrency.CurrencyISO ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@currencyCode", oCurrency.CurrencyCode));
            cmd.Parameters.Add(new SqlParameter("@currencyDescription", oCurrency.CurrencyDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@currencyCreatorId", oCurrency.CurrencyCreatorId));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditarCurrencyAsync(Currencies oCurrency)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Currencies", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@currencyId", oCurrency.CurrencyId));
            cmd.Parameters.Add(new SqlParameter("@currencyName", oCurrency.CurrencyName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@currencyISO", oCurrency.CurrencyISO ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@currencyCode", oCurrency.CurrencyCode));
            cmd.Parameters.Add(new SqlParameter("@currencyDescription", oCurrency.CurrencyDescription ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@currencyModificatorId", oCurrency.CurrencyModificatorId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarCurrencyAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_CATALOGS.sp_Tbl_Currencies", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@currencyId", id));
            cmd.Parameters.Add(new SqlParameter("@currencyModificatorId", idModificador));
            cmd.Parameters.Add(new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output });
            await cmd.ExecuteNonQueryAsync();
        }
    }
}