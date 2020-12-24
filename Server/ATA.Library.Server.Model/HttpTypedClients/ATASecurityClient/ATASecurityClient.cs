using System.Net.Http;

namespace ATA.Library.Server.Model.HttpTypedClients.ATASecurityClient
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