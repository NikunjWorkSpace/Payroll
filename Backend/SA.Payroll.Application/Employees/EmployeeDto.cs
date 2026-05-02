using SA.Payroll.Core.Enums;

namespace SA.Payroll.Application.Employees;

public sealed record EmployeeDto(
    Guid Id,
    Guid TenantId,
    Guid CompanyId,
    Guid DepartmentId,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string IdNumber,
    string Position,
    decimal BasicSalary,
    EmploymentStatus Status,
    decimal AnnualLeaveDays,
    decimal SickLeaveDays,
    decimal UnpaidLeaveDays);

