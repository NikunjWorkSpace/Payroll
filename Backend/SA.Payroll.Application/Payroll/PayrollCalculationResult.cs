namespace SA.Payroll.Application.Payroll;

public sealed record PayrollCalculationResult(
    decimal GrossPay,
    decimal TaxableEarnings,
    decimal Paye,
    decimal UifEmployee,
    decimal SdlEmployer,
    decimal CustomDeductions,
    decimal NetPay,
    IReadOnlyDictionary<string, decimal> Breakdown);

