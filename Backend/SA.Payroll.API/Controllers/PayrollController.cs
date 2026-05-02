using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Payroll.Application.Payroll;

namespace SA.Payroll.API.Controllers;

[ApiController]
[Authorize(Roles = "GlobalAdmin,CompanyAdmin,PayrollOfficer")]
[Route("api/[controller]")]
public sealed class PayrollController : ControllerBase
{
    private readonly IPayrollService _payrollService;

    public PayrollController(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    [HttpGet("history")]
    [ProducesResponseType(typeof(IReadOnlyList<PayrollRunDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PayrollRunDto>>> GetHistory(CancellationToken cancellationToken)
    {
        var history = await _payrollService.GetHistoryAsync(cancellationToken);
        return Ok(history);
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(PayrollSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PayrollSummaryDto>> GetSummary(CancellationToken cancellationToken)
    {
        var summary = await _payrollService.GetSummaryAsync(cancellationToken);
        return Ok(summary);
    }

    [HttpPost("process")]
    [ProducesResponseType(typeof(PayrollRunDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PayrollRunDto>> Process([FromBody] ProcessPayrollRequest request, CancellationToken cancellationToken)
    {
        var run = await _payrollService.ProcessAsync(request, cancellationToken);
        return Ok(run);
    }
}

