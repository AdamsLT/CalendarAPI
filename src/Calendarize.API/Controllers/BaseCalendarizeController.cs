using Microsoft.AspNetCore.Mvc;

namespace Calendarize.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseCalendarizeController : ControllerBase
    {
    }
}
