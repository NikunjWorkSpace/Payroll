using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Payroll.Application.Employees;

namespace SA.Payroll.API.Controllers;

[ApiController]
[Authorize(Roles = "GlobalAdmin,CompanyAdmin,PayrollOfficer")]
[Route("api/[controller]")]
public sealed class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> Get(CancellationToken cancellationToken)
    {
        var employees = await _employeeService.GetByTenantAsync(cancellationToken);
        return Ok(employees);
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var employee = await _employeeService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    [HttpPut("{employeeId:guid}/leave-balance")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<EmployeeDto>> UpdateLeaveBalance(
        Guid employeeId,
        [FromBody] UpdateLeaveBalanceRequest request,
        CancellationToken cancellationToken)
    {
        var employee = await _employeeService.UpdateLeaveBalanceAsync(employeeId, request, cancellationToken);
        return Ok(employee);
    }
}

