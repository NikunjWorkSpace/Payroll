namespace SA.Payroll.Core.ValueObjects;

public sealed record BankAccountDetails(
    string BankName,
    string AccountNumber,
    string BranchCode,
    string AccountType);

