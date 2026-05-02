using SA.Payroll.Core.Common;
using SA.Payroll.Core.Enums;

namespace SA.Payroll.Core.Entities;

public sealed class Company : AuditableEntity, ITenantOwned
{
    public Guid TenantId { get; set; }

    public Guid? ParentCompanyId { get; set; }

    public string RegisteredName { get; set; } = string.Empty;

    public string RegistrationNumber { get; set; } = string.Empty;

    public CompanyType CompanyType { get; set; } = CompanyType.Subsidiary;

    public PayrollCycle DefaultPayrollCycle { get; set; } = PayrollCycle.Monthly;

    public string ContactEmail { get; set; } = string.Empty;

    public string CountryCode { get; set; } = "ZA";

    public bool IsActive { get; set; } = true;

    public decimal UifRate { get; set; } = 0.01m;

    public decimal SdlRate { get; set; } = 0.01m;

    public ICollection<Department> Departments { get; set; } = new List<Department>();

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public ICollection<PayrollRun> PayrollRuns { get; set; } = new List<PayrollRun>();
}

