namespace Data.Models.Bases;

public abstract class BaseId
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
