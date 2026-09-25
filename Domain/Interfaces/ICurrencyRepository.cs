using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currencies>> ListarCurrenciesAsync();
        Task<IEnumerable<Currencies>> ListarCurrenciesFiltroAsync(string filtro);
        Task NuevoCurrencyAsync(Currencies oCurrency);
        Task EditarCurrencyAsync(Currencies oCurrency);
        Task EliminarCurrencyAsync(int id, int idModificador);
    }
}
