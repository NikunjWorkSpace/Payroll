using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Payroll.Application.Payroll;

namespace SA.Payroll.API.Controllers;

[ApiController]
[Authorize(Roles = "GlobalAdmin,CompanyAdmin,PayrollOfficer,Employee")]
[Route("api/[controller]")]
public sealed class ReportsController : ControllerBase
{
    private readonly IPayrollService _payrollService;

    public ReportsController(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    [HttpGet("statutory-summary")]
    [ProducesResponseType(typeof(PayrollSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PayrollSummaryDto>> GetStatutorySummary(CancellationToken cancellationToken)
    {
        var summary = await _payrollService.GetSummaryAsync(cancellationToken);
        return Ok(summary);
    }

    [HttpGet("payslips/{payrollLineId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DownloadPayslip(Guid payrollLineId, CancellationToken cancellationToken)
    {
        var document = await _payrollService.GetPayslipAsync(payrollLineId, cancellationToken);
        return File(document.Content, document.ContentType, document.FileName);
    }
}

