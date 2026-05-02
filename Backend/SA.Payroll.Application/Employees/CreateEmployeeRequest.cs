namespace SA.Payroll.Application.Employees;

public sealed record CreateEmployeeRequest(
    Guid CompanyId,
    Guid DepartmentId,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string IdNumber,
    string Email,
    string Position,
    DateOnly HireDate,
    decimal BasicSalary,
    string BankName,
    string AccountNumber,
    string BranchCode,
    string AccountType,
    string TaxNumber,
    string IncomeTaxReference,
    bool IsUifExempt,
    bool IsSdlExempt);

