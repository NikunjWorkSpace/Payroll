namespace SA.Payroll.Application.Payroll;

public sealed record PayrollSummaryDto(
    int ActiveEmployees,
    decimal LatestGrossTotal,
    decimal LatestDeductions,
    decimal LatestNetTotal,
    DateTime? LatestProcessedUtc);

