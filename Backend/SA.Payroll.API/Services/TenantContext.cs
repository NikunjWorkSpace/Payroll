using SA.Payroll.Application.Abstractions;

namespace SA.Payroll.API.Services;

public sealed class TenantContext : ITenantContext
{
    public Guid? TenantId { get; private set; }

    public void SetTenant(Guid? tenantId)
    {
        TenantId = tenantId;
    }
}

