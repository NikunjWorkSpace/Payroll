using SA.Payroll.Core.Common;

namespace SA.Payroll.Core.Entities;

public sealed class PayrollLine : AuditableEntity, ITenantOwned
{
    public Guid TenantId { get; set; }

    public Guid PayrollRunId { get; set; }

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public decimal GrossPay { get; set; }

    public decimal TaxableEarnings { get; set; }

    public decimal Paye { get; set; }

    public decimal UifEmployee { get; set; }

    public decimal SdlEmployer { get; set; }

    public decimal CustomDeductions { get; set; }

    public decimal NetPay { get; set; }

    public string? Notes { get; set; }

    public PayrollRun? PayrollRun { get; set; }

    public Employee? Employee { get; set; }
}

