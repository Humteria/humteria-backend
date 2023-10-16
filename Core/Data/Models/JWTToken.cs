using System.ComponentModel.DataAnnotations;

namespace Humteria.Data.Models;

public class JwtToken
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
}
