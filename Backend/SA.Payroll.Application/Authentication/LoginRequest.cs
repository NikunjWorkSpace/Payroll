namespace SA.Payroll.Application.Authentication;

public sealed record LoginRequest(
    string Email,
    string Password);

