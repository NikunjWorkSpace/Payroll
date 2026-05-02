using SA.Payroll.Core.Common;
using SA.Payroll.Core.Enums;

namespace SA.Payroll.Core.Entities;

public sealed class SalaryComponent : AuditableEntity, ITenantOwned
{
    public Guid TenantId { get; set; }

    public Guid EmployeeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public SalaryComponentType Type { get; set; }

    public decimal Amount { get; set; }

    public bool IsRecurring { get; set; } = true;

    public bool IsTaxable { get; set; } = true;

    public DateOnly EffectiveFrom { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public Employee? Employee { get; set; }
}
