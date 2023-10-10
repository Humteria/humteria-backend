using System.ComponentModel.DataAnnotations;

namespace Data.Models.ModelBases;

public abstract class ModelBaseTitle : ModelBaseId
{
    [Required]
    [MaxLength(64)]
    public string Title { get; set; } = string.Empty;
}
