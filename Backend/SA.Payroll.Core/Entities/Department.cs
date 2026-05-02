using SA.Payroll.Core.Common;

namespace SA.Payroll.Core.Entities;

public sealed class Department : AuditableEntity, ITenantOwned
{
    public Guid TenantId { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? CostCenter { get; set; }

    public Company? Company { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

