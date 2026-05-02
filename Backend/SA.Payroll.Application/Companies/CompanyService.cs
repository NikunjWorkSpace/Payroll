using Microsoft.EntityFrameworkCore;
using SA.Payroll.Application.Abstractions;
using SA.Payroll.Core.Entities;
using SA.Payroll.Core.Enums;

namespace SA.Payroll.Application.Companies;

public sealed class CompanyService : ICompanyService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public CompanyService(IApplicationDbContext dbContext, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<CompanyDto>> GetAccessibleCompaniesAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Company> query = _dbContext.Companies.AsNoTracking();

        if (_currentUser.IsInRole(nameof(UserRole.GlobalAdmin)) && _currentUser.ParentCompanyId.HasValue)
        {
            var parentCompanyId = _currentUser.ParentCompanyId.Value;
            query = query.Where(x => x.Id == parentCompanyId || x.ParentCompanyId == parentCompanyId);
        }
        else if (_currentUser.TenantId.HasValue)
        {
            var tenantId = _currentUser.TenantId.Value;
            query = query.Where(x => x.TenantId == tenantId);
        }
        else
        {
            query = query.Where(_ => false);
        }

        var companies = await query
            .OrderBy(x => x.RegisteredName)
            .ToListAsync(cancellationToken);

        return companies.Select(Map).ToList();
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsInRole(nameof(UserRole.GlobalAdmin)))
        {
            throw new UnauthorizedAccessException("Only global admins can create companies.");
        }

        var companyId = Guid.NewGuid();
        var company = new Company
        {
            Id = companyId,
            TenantId = companyId,
            ParentCompanyId = request.ParentCompanyId,
            RegisteredName = request.RegisteredName,
            RegistrationNumber = request.RegistrationNumber,
            ContactEmail = request.ContactEmail,
            CompanyType = request.CompanyType,
            DefaultPayrollCycle = request.DefaultPayrollCycle,
            UifRate = request.UifRate <= 0 ? 0.01m : request.UifRate,
            SdlRate = request.SdlRate <= 0 ? 0.01m : request.SdlRate
        };

        _dbContext.Companies.Add(company);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Map(company);
    }

    private static CompanyDto Map(Company company) =>
        new(
            company.Id,
            company.TenantId,
            company.ParentCompanyId,
            company.RegisteredName,
            company.RegistrationNumber,
            company.CompanyType,
            company.DefaultPayrollCycle,
            company.ContactEmail,
            company.IsActive);
}
