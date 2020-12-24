using ATA.Library.Server.Model.Entities.Book;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.Library.Server.Model.Entities.Category
{
    [Table("Categories")]
    public class CategoryEntity : ATAEntity
    {
        [Required]
        public string? CategoryName { get; set; }

        public string? AccessRole { get; set; }


        public ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
    }
}