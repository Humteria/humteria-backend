namespace Humteria.Data.Models.Authentication;

public class AuthenticatedUser
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? AccessToken { get; set; }
}
