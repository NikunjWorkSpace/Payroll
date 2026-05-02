using SA.Payroll.Core.Enums;

namespace SA.Payroll.Application.Companies;

public sealed record CompanyDto(
    Guid Id,
    Guid TenantId,
    Guid? ParentCompanyId,
    string RegisteredName,
    string RegistrationNumber,
    CompanyType CompanyType,
    PayrollCycle DefaultPayrollCycle,
    string ContactEmail,
    bool IsActive);

