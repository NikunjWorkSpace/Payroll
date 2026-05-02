using SA.Payroll.Core.Enums;

namespace SA.Payroll.Application.Payroll;

public sealed record PayrollRunDto(
    Guid Id,
    string RunNumber,
    Guid TenantId,
    Guid CompanyId,
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    PayrollCycle Cycle,
    PayrollRunStatus Status,
    decimal GrossTotal,
    decimal TotalDeductions,
    decimal NetTotal,
    DateTime ProcessedUtc,
    IReadOnlyList<PayrollLineDto> PayrollLines);

