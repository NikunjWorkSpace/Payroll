using SA.Payroll.Core.Enums;

namespace SA.Payroll.Application.Companies;

public sealed record CreateCompanyRequest(
    string RegisteredName,
    string RegistrationNumber,
    string ContactEmail,
    Guid? ParentCompanyId,
    CompanyType CompanyType,
    PayrollCycle DefaultPayrollCycle,
    decimal UifRate,
    decimal SdlRate);

