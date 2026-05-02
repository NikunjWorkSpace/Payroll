using SA.Payroll.Core.Enums;

namespace SA.Payroll.Application.Payroll;

public sealed record ProcessPayrollRequest(
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    PayrollCycle Cycle);

