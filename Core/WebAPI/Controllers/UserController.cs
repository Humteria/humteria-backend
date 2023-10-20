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
        var validationContext = new ValidationContext(user, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();
        // Validate the user object
        if (!Validator.TryValidateObject(user, validationContext, validationResults, validateAllProperties: true))
        {
            // Validation failed, return a BadRequest response with the validation errors
            return BadRequest(validationResults);
        }

        // If validation succeeds, continue with creating the user
        // TODO: User insertion into DB
        return Ok("User created successfully");
    }
}
