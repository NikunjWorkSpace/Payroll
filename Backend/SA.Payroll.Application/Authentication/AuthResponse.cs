namespace SA.Payroll.Application.Authentication;

public sealed record AuthResponse(
    string AccessToken,
    string FullName,
    string Email,
    string Role,
    Guid? ParentCompanyId,
    Guid? TenantId);

