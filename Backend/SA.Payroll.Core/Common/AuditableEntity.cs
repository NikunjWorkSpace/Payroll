namespace SA.Payroll.Core.Common;

public abstract class AuditableEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedUtc { get; set; }
}

