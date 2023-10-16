using Humteria.Data.Models.Bases;

namespace Humteria.Data.DTOs.UserDTO;

public class JWTUserForTokenDTO : BaseId
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
