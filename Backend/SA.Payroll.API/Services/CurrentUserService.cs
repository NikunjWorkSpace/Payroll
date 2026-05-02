using System.Security.Claims;
using SA.Payroll.Application.Abstractions;

namespace SA.Payroll.API.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId => ParseGuid(ClaimTypes.NameIdentifier) ?? ParseGuid("sub");

    public Guid? ParentCompanyId => ParseGuid("parent_company_id");

    public Guid? TenantId => ParseGuid("tenant_id");

    public string Email => GetValue(ClaimTypes.Email) ?? GetValue("email") ?? string.Empty;

    public string FullName => GetValue(ClaimTypes.Name) ?? GetValue("name") ?? string.Empty;

    public string Role => GetValue(ClaimTypes.Role) ?? string.Empty;

    public bool IsInRole(string role) =>
        string.Equals(Role, role, StringComparison.OrdinalIgnoreCase);

    private string? GetValue(string claimType) =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);

    private Guid? ParseGuid(string claimType)
    {
        var raw = GetValue(claimType);
        return Guid.TryParse(raw, out var parsed) ? parsed : null;
    }
}

