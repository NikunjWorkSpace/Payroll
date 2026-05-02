namespace SA.Payroll.Core.ValueObjects;

public sealed record TaxProfile(
    string TaxNumber,
    string IncomeTaxReference,
    bool IsUifExempt,
    bool IsSdlExempt);

