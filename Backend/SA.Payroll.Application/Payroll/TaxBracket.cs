namespace SA.Payroll.Application.Payroll;

public sealed record TaxBracket(
    decimal Threshold,
    decimal BaseTax,
    decimal Rate);
