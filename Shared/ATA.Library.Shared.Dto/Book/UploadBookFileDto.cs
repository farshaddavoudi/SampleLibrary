using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ATA.Library.Shared.Core;

namespace ATA.Library.Shared.Dto
{
    public class UploadBookFileDto : IValidatableObject
    {
        [Required]
        public string? BookName { get; set; }

        [Required]
        public byte[]? BookData { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BookData!.Length > AppStrings.UploadLimits.MaxBookFileSizeInMB * 1000000)
                yield return new ValidationResult("حجم فایل کتاب بیشتر از حد مجاز می‌باشد",
                    new[] { nameof(BookData) });
        }
    }
}