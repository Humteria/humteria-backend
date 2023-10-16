namespace Data.Models;

public class JwtToken
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
}