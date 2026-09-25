using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infraestructure.Repositories
{
    public class ProductVariableRepository : IProductVariableRepository
    {
        private readonly DbConection _conexion;

        public ProductVariableRepository(DbConection conexion)
        {
            _conexion = conexion;
        }

        /// <summary>
        /// Indica si el result set trae la columna. `Mapear` lo comparten los
        /// modos LST, GET y FIL del SP, y no todos devuelven las mismas
        /// columnas: leer una ausente lanza IndexOutOfRangeException.
        /// </summary>
        public async Task ActualizarDescuentoAsync(int productVariableId, decimal descuento,
                                                   DateTime? hasta, int modificadorId)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_ActualizarDescuentoVariante", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@productVariableId", productVariableId));
            cmd.Parameters.Add(new SqlParameter("@descuento", descuento));
            cmd.Parameters.Add(new SqlParameter("@hasta", (object?)hasta ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@modificadorId", modificadorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);

            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) <= 0)
                throw new Exception(oMsg.Value?.ToString() ?? "No se pudo aplicar el descuento.");
        }

        private static bool TieneColumna(SqlDataReader dr, string nombre)
        {
            for (var i = 0; i < dr.FieldCount; i++)
                if (string.Equals(dr.GetName(i), nombre, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private ProductVariable Mapear(SqlDataReader dr) => new ProductVariable
        {
            ProductVariableId = Convert.ToInt32(dr["productVariableId"]),
            ProductVariableProductId = Convert.ToInt32(dr["productVariableProductId"]),
            ProductVariableDiscount = TieneColumna(dr, "productVariableDiscount") && dr["productVariableDiscount"] != DBNull.Value ? Convert.ToDecimal(dr["productVariableDiscount"]) : 0m,
            ProductName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
            ProductVariableValue = dr["productVariableValue"] != DBNull.Value ? dr["productVariableValue"].ToString() : null,
            ProductVariablePrice = Convert.ToDecimal(dr["productVariablePrice"]),
            ProductVariableCurrencyId = Convert.ToInt32(dr["productVariableCurrencyId"]),
            CurrencyISO = dr["currencyISO"] != DBNull.Value ? dr["currencyISO"].ToString() : null,
            CurrencyName = dr["currencyName"] != DBNull.Value ? dr["currencyName"].ToString() : null,
            StockDisponible = Convert.ToInt32(dr["stockDisponible"]),
            VariableTypeName = TieneColumna(dr, "variableTypeName") && dr["variableTypeName"] != DBNull.Value
                ? dr["variableTypeName"].ToString()
                : null,
            ProductVariableCreatorId = Convert.ToInt32(dr["productVariableCreatorId"]),
            ProductVariableCreationDate = dr["productVariableCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productVariableCreationDate"]) : null,
            ProductVariableModificatorId = dr["productVariableModificatorId"] != DBNull.Value ? Convert.ToInt32(dr["productVariableModificatorId"]) : null,
            ProductVariableModificationDate = dr["productVariableModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["productVariableModificationDate"]) : null,
            ProductVariableStatusId = Convert.ToBoolean(dr["productVariableStatusId"])
        };

        public async Task<IEnumerable<ProductVariable>> ListarAsync(int? productId = null, bool soloActivos = true)
        {
            var lista = new List<ProductVariable>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "LST"));
            cmd.Parameters.Add(new SqlParameter("@productVariableProductId",
                productId.HasValue ? (object)productId.Value : DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@soloActivos", soloActivos ? 1 : 0));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<IEnumerable<ProductVariable>> FiltrarAsync(string filtro)
        {
            var lista = new List<ProductVariable>();
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "FIL"));
            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(Mapear(dr));
            return lista;
        }

        public async Task<ProductVariable?> ObtenerPorIdAsync(int id)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "GET"));
            cmd.Parameters.Add(new SqlParameter("@productVariableId", id));
            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync()) return Mapear(dr);
            return null;
        }

        public async Task NuevaVariableAsync(ProductVariable entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "INS"));
            cmd.Parameters.Add(new SqlParameter("@productVariableProductId", entity.ProductVariableProductId));
            cmd.Parameters.Add(new SqlParameter("@productVariableValue", entity.ProductVariableValue ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productVariablePrice", entity.ProductVariablePrice));
            cmd.Parameters.Add(new SqlParameter("@productVariableCurrencyId", entity.ProductVariableCurrencyId));
            cmd.Parameters.Add(new SqlParameter("@productVariableCreatorId", entity.ProductVariableCreatorId));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EditarVariableAsync(ProductVariable entity)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "UPD"));
            cmd.Parameters.Add(new SqlParameter("@productVariableId", entity.ProductVariableId));
            // Editar una variante no cambia de producto, así que la aplicación no
            // envía ese id. Al ser un entero no anulable llegaba como 0, y el
            // procedimiento rechazaba la edición entera con "el nuevo producto no
            // existe": cero no es un producto. Sin valor, el procedimiento
            // conserva el que ya tenía.
            cmd.Parameters.Add(new SqlParameter("@productVariableProductId",
                entity.ProductVariableProductId > 0 ? entity.ProductVariableProductId : (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productVariableValue", entity.ProductVariableValue ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@productVariablePrice", entity.ProductVariablePrice));
            cmd.Parameters.Add(new SqlParameter("@productVariableCurrencyId", entity.ProductVariableCurrencyId));
            cmd.Parameters.Add(new SqlParameter("@productVariableModificatorId", entity.ProductVariableModificatorId ?? (object)DBNull.Value));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }

        public async Task EliminarVariableAsync(int id, int idModificador)
        {
            using var con = _conexion.CreateConection();
            await con.OpenAsync();
            using var cmd = new SqlCommand("SQM_GENERAL.sp_Tbl_ProductVariables", con)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Mode", "DEL"));
            cmd.Parameters.Add(new SqlParameter("@productVariableId", id));
            cmd.Parameters.Add(new SqlParameter("@productVariableModificatorId", idModificador));
            var oMsg = new SqlParameter("@O_Msg", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
            var oNum = new SqlParameter("@O_Num", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(oMsg);
            cmd.Parameters.Add(oNum);
            await cmd.ExecuteNonQueryAsync();
            if (Convert.ToInt32(oNum.Value) == -1) throw new Exception(oMsg.Value?.ToString());
        }
    }
}