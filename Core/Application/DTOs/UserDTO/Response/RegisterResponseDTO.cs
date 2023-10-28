using Humteria.Data.Models.Bases;

namespace Humteria.Application.DTOs.UserDTO.Response;

public class RegisterResponseDTO : BaseId
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public string Token { get; set; } = string.Empty;
}
