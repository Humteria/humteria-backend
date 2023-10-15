using Data.Models.Bases;

namespace Data.DTOs.UserDTO;

public class LoginResponseDTO : BaseId
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Token { get; set; } = string.Empty;
}
