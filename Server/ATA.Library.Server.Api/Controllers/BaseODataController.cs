using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATA.Library.Server.Api.Controllers
{
    [Route("odata/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [EnableQuery]
    [Authorize] //It needed to be here due to SSO token param in Swagger be visible
    public abstract class BaseODataController : ODataController
    {
    }
}