namespace SA.Payroll.Application.Companies;

public interface ICompanyService
{
    Task<IReadOnlyList<CompanyDto>> GetAccessibleCompaniesAsync(CancellationToken cancellationToken = default);

    Task<CompanyDto> CreateAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default);
}

