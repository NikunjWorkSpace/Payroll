namespace SA.Payroll.Infrastructure.Services;

public sealed class DemoUserCredential
{
    public Guid UserId { get; set; } = Guid.NewGuid();

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public Guid? ParentCompanyId { get; set; }

    public Guid? TenantId { get; set; }
}

