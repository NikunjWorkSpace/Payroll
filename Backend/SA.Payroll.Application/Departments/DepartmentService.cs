using Microsoft.EntityFrameworkCore;
using SA.Payroll.Application.Abstractions;

namespace SA.Payroll.Application.Departments;

public sealed class DepartmentService : IDepartmentService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ITenantContext _tenantContext;

    public DepartmentService(IApplicationDbContext dbContext, ITenantContext tenantContext)
    {
        _dbContext = dbContext;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetByTenantAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = _tenantContext.TenantId
            ?? throw new InvalidOperationException("Tenant context is required for department operations.");

        var departments = await _dbContext.Departments
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return departments
            .Select(x => new DepartmentDto(x.Id, x.TenantId, x.CompanyId, x.Name, x.CostCenter))
            .ToList();
    }
}

