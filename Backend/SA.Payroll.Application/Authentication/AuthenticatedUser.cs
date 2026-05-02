namespace SA.Payroll.Application.Authentication;

public sealed record AuthenticatedUser(
    Guid UserId,
    string FullName,
    string Email,
    string Role,
    Guid? ParentCompanyId,
    Guid? TenantId);

