using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplication.DTOs
{
    // Resultado paginado genérico. HasMore indica si el frontend debe
    // pedir la siguiente página (scroll infinito).
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalRows / PageSize) : 0;
        public bool HasMore => (long)PageNumber * PageSize < TotalRows;
    }
}
