using Humteria.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Humteria.WebAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        return Ok("User created successfully");
    }
}
