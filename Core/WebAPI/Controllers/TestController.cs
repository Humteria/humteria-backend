using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Humteria.Core.Controllers
{
    [Route("api/test")]
    [Authorize]
    [ApiController]
    public class TestController : ControllerBase
    {
        private List<string> users = new List<string>
        {
            "User1",
            "User2",
            "User3"
        };

        [HttpGet("input")]
        public IActionResult Get()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("You must be authenticated to access this resource.");
            }
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("register")]
        public IActionResult Register()
        {
            return Ok(users);
        }
    }
}