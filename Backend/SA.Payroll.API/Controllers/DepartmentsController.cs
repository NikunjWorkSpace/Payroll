using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Payroll.Application.Departments;

namespace SA.Payroll.API.Controllers;

[ApiController]
[Authorize(Roles = "GlobalAdmin,CompanyAdmin,PayrollOfficer")]
[Route("api/[controller]")]
public sealed class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DepartmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<DepartmentDto>>> Get(CancellationToken cancellationToken)
    {
        var departments = await _departmentService.GetByTenantAsync(cancellationToken);
        return Ok(departments);
    }
}
