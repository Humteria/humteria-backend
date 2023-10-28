using System.ComponentModel.DataAnnotations;
using Humteria.Data.Models.ModelBases;

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
