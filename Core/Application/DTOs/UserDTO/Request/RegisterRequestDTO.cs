using System.ComponentModel.DataAnnotations;

namespace Humteria.Application.DTOs.UserDTO.Request;

public class RegisterRequestDTO
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

    //TODO maybe take out
    [Required, Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string PasswordConfirmation { get; set; } = string.Empty;
}