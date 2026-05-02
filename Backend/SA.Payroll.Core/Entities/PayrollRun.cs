using SA.Payroll.Core.Common;
using SA.Payroll.Core.Enums;

namespace SA.Payroll.Core.Entities;

public sealed class PayrollRun : AuditableEntity, ITenantOwned
{
    public Guid TenantId { get; set; }

    public Guid CompanyId { get; set; }

    public DateOnly PeriodStart { get; set; }

    public DateOnly PeriodEnd { get; set; }

    public string RunNumber { get; set; } = string.Empty;

    public PayrollCycle Cycle { get; set; } = PayrollCycle.Monthly;

    public PayrollRunStatus Status { get; set; } = PayrollRunStatus.Draft;

    public decimal GrossTotal { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal NetTotal { get; set; }

    public DateTime ProcessedUtc { get; set; } = DateTime.UtcNow;

    public Company? Company { get; set; }

    public ICollection<PayrollLine> PayrollLines { get; set; } = new List<PayrollLine>();
}

