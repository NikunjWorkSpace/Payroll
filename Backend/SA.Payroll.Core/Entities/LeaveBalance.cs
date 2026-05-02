using SA.Payroll.Core.Common;

namespace SA.Payroll.Core.Entities;

public sealed class LeaveBalance : AuditableEntity, ITenantOwned
{
    public Guid TenantId { get; set; }

    public Guid EmployeeId { get; set; }

    public decimal AnnualLeaveDays { get; set; } = 15m;

    public decimal SickLeaveDays { get; set; } = 10m;

    public decimal UnpaidLeaveDays { get; set; }

    public decimal CarriedForwardDays { get; set; }

    public Employee? Employee { get; set; }
}

