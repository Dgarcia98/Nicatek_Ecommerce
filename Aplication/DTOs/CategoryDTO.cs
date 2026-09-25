using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public int CategoryCreatorId { get; set; }
        public DateTime? CategoryCreationDate { get; set; }
        public int? CategoryModificatorId { get; set; }
        public DateTime? CategoryModificationDate { get; set; }
        public bool CategoryStatusId { get; set; }
    }
}
