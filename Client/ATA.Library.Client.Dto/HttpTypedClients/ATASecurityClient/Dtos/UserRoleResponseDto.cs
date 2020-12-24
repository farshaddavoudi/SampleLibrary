namespace ATA.Library.Client.Dto.HttpTypedClients.ATASecurityClient.Dtos
{
    public class UserRoleResponseDto
    {
        public int RoleID { get; set; }

        public string? RoleTitle { get; set; }

        public bool IsAdmin { get; set; }

        public string? Tag { get; set; }
    }
}