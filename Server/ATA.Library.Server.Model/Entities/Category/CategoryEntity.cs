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
    }
}