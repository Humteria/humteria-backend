using Humteria.Data.Models.ModelBases;

namespace Humteria.Data.Models;

public class User : ModelBaseId
{
    [Required]
    public string FirstName {  get; set; } = string.Empty;
    [Required] 
    public string LastName { get; set;  } = string.Empty;
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string PasswordResetToken { get; set; } = string.Empty;
    [Required]
    public bool IsEmailConfirmed { get; set; } = false;
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsSystemAdmin { get; set; } = false;
    public JwtToken? JwtToken { get; set; }

}
