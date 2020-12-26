using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ATA.Library.Shared.Dto;

namespace ATA.Library.Server.Model.Book
{
    public class BookDto : IATADto
    {
        [Key]
        public int Id { get; set; }

        public bool IsArchived { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }


        [Required(ErrorMessage = "گروه را وارد نمایید")]
        public int CategoryId { get; set; }

        public bool IsDownloadable { get; set; } = true;

        public List<FileData> FileData { get; set; } = new List<FileData>();

        public string? CoverImageUrl { get; set; }


        [Required(ErrorMessage = "عنوان کتاب را وارد نمایید")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Author { get; set; }

        public string? FileUrl { get; set; }

        public CategoryDto? Category { get; set; }

        public override string ToString()
        {
            return $"Book Id and name: {Id} | {Title}";
        }
    }
}