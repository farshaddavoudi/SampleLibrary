namespace ATA.Library.Client.Dto.HttpTypedClients.ATASecurityClient.Dtos
{
    public class UserByTokenData
    {
        public int UserID { get; set; }

        public string? FName { get; set; }

        public string? LName { get; set; }

        public int InfperID { get; set; }

        public int InfperCode { get; set; }

        public string? JobTitle { get; set; }

        public string? UnitTitle { get; set; }

        public string? Token { get; set; }

        public string? Username { get; set; }
    }
}