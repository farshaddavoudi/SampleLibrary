using System.ComponentModel.DataAnnotations;

namespace ATA.Library.Shared.Dto
{
    public partial class UserDto : IDto
    {
        [Key]
        public int Id { get; set; }

        public string? SsoToken { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int PersonnelCode { get; set; }

        public string? PostTitle { get; set; }

        public int? WorkLocationCode { get; set; }

        public string? WorkLocation { get; set; }

    }
}
