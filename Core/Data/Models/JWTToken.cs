using Data.Models.ModelBases;
using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class JwtToken : ModelBaseId
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
}
