using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Payroll.Application.Companies;

namespace SA.Payroll.API.Controllers;

[ApiController]
[Authorize(Roles = "GlobalAdmin,CompanyAdmin,PayrollOfficer")]
[Route("api/[controller]")]
public sealed class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CompanyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CompanyDto>>> Get(CancellationToken cancellationToken)
    {
        var companies = await _companyService.GetAccessibleCompaniesAsync(cancellationToken);
        return Ok(companies);
    }

    [Authorize(Roles = "GlobalAdmin")]
    [HttpPost]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CompanyDto>> Create([FromBody] CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await _companyService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = company.Id }, company);
    }
}

