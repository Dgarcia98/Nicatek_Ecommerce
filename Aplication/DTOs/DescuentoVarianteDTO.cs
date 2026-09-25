using System;

namespace Aplication.DTOs
{
    /// <summary>Descuento a aplicar sobre una variante. Cero lo retira.</summary>
    public class DescuentoVarianteDTO
    {
        public decimal Descuento { get; set; }
        /// <summary>Fin de la oferta. Null = sin fecha de caducidad.</summary>
        public DateTime? Hasta { get; set; }
        public int ModificadorId { get; set; }
    }
}
