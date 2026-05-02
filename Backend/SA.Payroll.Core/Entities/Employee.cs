using SA.Payroll.Core.Common;
using SA.Payroll.Core.Enums;
using SA.Payroll.Core.ValueObjects;

namespace SA.Payroll.Core.Entities;

public sealed class Employee : AuditableEntity, ITenantOwned
{
    public Guid TenantId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid DepartmentId { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string IdNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public DateOnly? TerminationDate { get; set; }

    public decimal BasicSalary { get; set; }

    public EmploymentStatus Status { get; set; } = EmploymentStatus.Active;

    public BankAccountDetails BankAccount { get; set; } = new(string.Empty, string.Empty, string.Empty, string.Empty);

    public TaxProfile TaxProfile { get; set; } = new(string.Empty, string.Empty, false, false);

    public Company? Company { get; set; }

    public Department? Department { get; set; }

    public LeaveBalance? LeaveBalance { get; set; }

    public ICollection<SalaryComponent> SalaryComponents { get; set; } = new List<SalaryComponent>();

    public ICollection<PayrollLine> PayrollLines { get; set; } = new List<PayrollLine>();

    public string FullName => $"{FirstName} {LastName}".Trim();
}

