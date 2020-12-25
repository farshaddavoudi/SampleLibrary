using System;
using System.ComponentModel.DataAnnotations;

namespace ATA.Library.Shared.Dto
{
    public partial class CategoryDto : IATADto
    {
        [Key]
        public int Id { get; set; }

        public bool IsArchived { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        [Required(ErrorMessage = "عنوان دسته را وارد نمایید")]
        public string? CategoryName { get; set; }

        public string? AdminRole { get; set; }
    }
}