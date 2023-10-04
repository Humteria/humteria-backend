using Microsoft.AspNetCore.Mvc;

namespace Humteria.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        NoContent();
}
