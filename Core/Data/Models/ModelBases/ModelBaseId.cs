using Humteria.Data.Models.Bases;
using System.ComponentModel.DataAnnotations;

namespace Humteria.Data.Models.ModelBases;

public abstract class ModelBaseId : BaseId
{
    [Key]
    [Required]
    public new Guid Id { get; set; } = Guid.NewGuid();
}
