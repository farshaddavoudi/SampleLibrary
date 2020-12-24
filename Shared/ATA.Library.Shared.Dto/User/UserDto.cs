using System.ComponentModel.DataAnnotations;
using ATA.Library.Client.Dto.Contracts;

namespace ATA.Library.Client.Dto.User
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
