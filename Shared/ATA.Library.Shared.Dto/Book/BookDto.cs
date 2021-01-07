using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ATA.Library.Shared.Core;

namespace ATA.Library.Shared.Dto
{
    public partial class BookDto : IATADto, IValidatableObject
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

        [StringLength(200, ErrorMessage = "حداکثر 200 کارکتر می‌توانید توضیحات وارد نمایید")]
        public string? Description { get; set; }

        public string? Author { get; set; }

        public string? BookFileFormat { get; set; }


        [Required(ErrorMessage = "هیچ کتابی انتخاب نشده است")]
        public string? BookFileUrl { get; set; }

        public long BookFileSize { get; set; }

        public CategoryDto? Category { get; set; }

        public override string ToString()
        {
            return $"Book Id and name: {Id} | {Title}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CoverImageByteData != null && CoverImageByteData.Length > AppStrings.UploadLimits.MaxCoverImageSizeInKB * 1000)
                yield return new ValidationResult("حجم عکس کتاب بیشتر از حد مجاز می‌باشد",
                    new[] { nameof(CoverImageByteData) });
        }
    }
}