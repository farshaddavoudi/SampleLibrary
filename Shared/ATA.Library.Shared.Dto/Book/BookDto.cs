using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ATA.Library.Shared.Dto;

namespace ATA.Library.Server.Model.Book
{
    public class BookDto : IATADto
    {
        [Key] public int Id { get; set; }

        public bool IsArchived { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }


        [Required(ErrorMessage = "گروه را وارد نمایید")]
        public int CategoryId { get; set; }

        public bool IsDownloadable { get; set; } = true;

        [NotMapped]
        public byte[]? CoverImageByteData { get; set; }

        public string? CoverImageFileFormat { get; set; }

        public string? CoverImageUrl { get; set; }


        [Required(ErrorMessage = "عنوان کتاب را وارد نمایید")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Author { get; set; }

        [NotMapped]
        public byte[]? BookFileByteData { get; set; }

        public string? BookFileFormat { get; set; }

        public string? BookFileUrl { get; set; }

        public long BookFileSize { get; set; }

        public CategoryDto? Category { get; set; }

        public override string ToString()
        {
            return $"Book Id and name: {Id} | {Title}";
        }
    }
}