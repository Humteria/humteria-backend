using System.ComponentModel.DataAnnotations;

namespace Data.DTOs.UserDTO;

public class LoginRequestDTO
{

    //TODO maybe remove required attribute from dtos
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
