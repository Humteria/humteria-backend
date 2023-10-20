using Humteria.Data.Models.ModelBases;
using System.ComponentModel.DataAnnotations;

namespace Humteria.Data.Models;

public class JwtToken : ModelBaseId
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    [Required]
    public string TokenType { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    //TODO ADD FK TO User
}
