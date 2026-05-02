namespace SA.Payroll.Application.Abstractions;

public interface ICurrentUserService
{
    Guid? UserId { get; }

    Guid? ParentCompanyId { get; }

    Guid? TenantId { get; }

    string Email { get; }

    string FullName { get; }

    string Role { get; }

    bool IsInRole(string role);
}

