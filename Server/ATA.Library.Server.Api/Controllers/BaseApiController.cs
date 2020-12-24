using ATA.Library.Server.Api.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATA.Library.Server.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    [ApiController]
    [ApiResultFilter]
    [Authorize] //It needed to be here due to SSO token param in Swagger be visible
    public abstract class BaseApiController : ControllerBase
    {
    }
}
