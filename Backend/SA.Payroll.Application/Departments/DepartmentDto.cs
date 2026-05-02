namespace SA.Payroll.Application.Departments;

public sealed record DepartmentDto(
    Guid Id,
    Guid TenantId,
    Guid CompanyId,
    string Name,
    string? CostCenter);

