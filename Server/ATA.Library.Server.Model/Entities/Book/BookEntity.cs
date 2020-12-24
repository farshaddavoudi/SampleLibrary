﻿using ATA.Library.Server.Model.Entities.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.Library.Server.Model.Entities.Book
{
    [Table("Books")]
    public class BookEntity : ATAEntity
    {
        [Required]
        public int CategoryId { get; set; }

        public bool IsDownloadable { get; set; }

        public string? CoverImageUrl { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Author { get; set; }

        public string? FileUrl { get; set; }


        [ForeignKey(nameof(CategoryId))]
        public CategoryEntity? Category { get; set; }
    }
}