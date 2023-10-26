namespace Humteria.Data.Models.ModelBases;

public class ModelBaseTitle : ModelBaseId
{
    [Required]
    [MaxLength(64)]
    public string Title { get; set; } = string.Empty;
}
