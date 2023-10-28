using System.ComponentModel.DataAnnotations;
using Humteria.Data.Models.Bases;

namespace Humteria.Data.Models.ModelBases;

public abstract class ModelBaseId : BaseId
{
    [Key]
    [Required]
    public new Guid Id { get; set; } = Guid.NewGuid();
}
