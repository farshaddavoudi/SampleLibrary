using System.Net.Http;

namespace ATA.Library.Client.Dto.HttpTypedClients.ATASecurityClient
{
    public class ATASecurityClient
    {
        public HttpClient Client { get; }

        public ATASecurityClient(HttpClient client)
        {
            Client = client;
        }
    }
}