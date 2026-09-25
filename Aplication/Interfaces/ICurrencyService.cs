using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyDTO>> ListarCurrencies();
        Task<CurrencyDTO?> ObtenerCurrencyPorId(int id);
        Task<IEnumerable<CurrencyDTO>> ListarPorNombre(string buscar);
        Task NuevoCurrency(CurrencyDTO dto);
        Task EditarCurrency(CurrencyDTO dto);
        Task EliminarCurrency(int id, int idModificador);
    }
}
