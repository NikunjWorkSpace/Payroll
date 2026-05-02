using Microsoft.EntityFrameworkCore;
using SA.Payroll.Application.Abstractions;
using SA.Payroll.Application.Common;
using SA.Payroll.Core.Entities;
using SA.Payroll.Core.Enums;

namespace SA.Payroll.Application.Payroll;

public sealed class PayrollService : IPayrollService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ITenantContext _tenantContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPayrollCalculator _payrollCalculator;
    private readonly IPdfRenderer _pdfRenderer;

    public PayrollService(
        IApplicationDbContext dbContext,
        ITenantContext tenantContext,
        IDateTimeProvider dateTimeProvider,
        IPayrollCalculator payrollCalculator,
        IPdfRenderer pdfRenderer)
    {
        _dbContext = dbContext;
        _tenantContext = tenantContext;
        _dateTimeProvider = dateTimeProvider;
        _payrollCalculator = payrollCalculator;
        _pdfRenderer = pdfRenderer;
    }

    public async Task<PayrollRunDto> ProcessAsync(ProcessPayrollRequest request, CancellationToken cancellationToken = default)
    {
        var tenantId = GetTenantId();
        var company = await _dbContext.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TenantId == tenantId, cancellationToken)
            ?? throw new KeyNotFoundException("Company settings could not be found for the selected tenant.");

        var employees = await _dbContext.Employees
            .Include(x => x.SalaryComponents)
            .Where(x => x.TenantId == tenantId && x.Status == EmploymentStatus.Active)
            .OrderBy(x => x.LastName)
            .ToListAsync(cancellationToken);

        if (employees.Count == 0)
        {
            throw new InvalidOperationException("No active employees were found for the selected tenant.");
        }

        var payrollRunId = Guid.NewGuid();
        var payrollRun = new PayrollRun
        {
            Id = payrollRunId,
            TenantId = tenantId,
            CompanyId = company.Id,
            PeriodStart = request.PeriodStart,
            PeriodEnd = request.PeriodEnd,
            Cycle = request.Cycle,
            Status = PayrollRunStatus.Processed,
            ProcessedUtc = _dateTimeProvider.UtcNow,
            RunNumber = $"PB-{request.PeriodStart:yyyyMM}-{_dateTimeProvider.UtcNow:HHmmss}"
        };

        foreach (var employee in employees)
        {
            var result = _payrollCalculator.Calculate(employee, company);
            payrollRun.PayrollLines.Add(new PayrollLine
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                PayrollRunId = payrollRunId,
                EmployeeId = employee.Id,
                EmployeeName = employee.FullName,
                GrossPay = result.GrossPay,
                TaxableEarnings = result.TaxableEarnings,
                Paye = result.Paye,
                UifEmployee = result.UifEmployee,
                SdlEmployer = result.SdlEmployer,
                CustomDeductions = result.CustomDeductions,
                NetPay = result.NetPay,
                Notes = string.Join("; ", result.Breakdown.Select(x => $"{x.Key}: {x.Value:0.00}"))
            });
        }

        payrollRun.GrossTotal = payrollRun.PayrollLines.Sum(x => x.GrossPay);
        payrollRun.TotalDeductions = payrollRun.PayrollLines.Sum(x => x.Paye + x.UifEmployee + x.CustomDeductions);
        payrollRun.NetTotal = payrollRun.PayrollLines.Sum(x => x.NetPay);

        _dbContext.PayrollRuns.Add(payrollRun);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Map(payrollRun);
    }

    public async Task<IReadOnlyList<PayrollRunDto>> GetHistoryAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = GetTenantId();

        var runs = await _dbContext.PayrollRuns
            .AsNoTracking()
            .Include(x => x.PayrollLines)
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.ProcessedUtc)
            .Take(12)
            .ToListAsync(cancellationToken);

        return runs.Select(Map).ToList();
    }

    public async Task<PayrollSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = GetTenantId();
        var activeEmployees = await _dbContext.Employees.CountAsync(
            x => x.TenantId == tenantId && x.Status == EmploymentStatus.Active,
            cancellationToken);

        var latestRun = await _dbContext.PayrollRuns
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.ProcessedUtc)
            .FirstOrDefaultAsync(cancellationToken);

        return new PayrollSummaryDto(
            activeEmployees,
            latestRun?.GrossTotal ?? 0,
            latestRun?.TotalDeductions ?? 0,
            latestRun?.NetTotal ?? 0,
            latestRun?.ProcessedUtc);
    }

    public async Task<PayslipDocument> GetPayslipAsync(Guid payrollLineId, CancellationToken cancellationToken = default)
    {
        var tenantId = GetTenantId();

        var payrollLine = await _dbContext.PayrollLines
            .AsNoTracking()
            .Include(x => x.PayrollRun)
            .FirstOrDefaultAsync(x => x.Id == payrollLineId && x.TenantId == tenantId, cancellationToken)
            ?? throw new KeyNotFoundException("Payslip line item could not be found.");

        var html = $"""
                    <html>
                    <body style="font-family:Segoe UI, Arial, sans-serif; padding:24px;">
                      <h1>PayBridge Payslip</h1>
                      <p><strong>Employee:</strong> {payrollLine.EmployeeName}</p>
                      <p><strong>Run Number:</strong> {payrollLine.PayrollRun?.RunNumber}</p>
                      <p><strong>Gross Pay:</strong> {payrollLine.GrossPay:0.00}</p>
                      <p><strong>PAYE:</strong> {payrollLine.Paye:0.00}</p>
                      <p><strong>UIF:</strong> {payrollLine.UifEmployee:0.00}</p>
                      <p><strong>Other Deductions:</strong> {payrollLine.CustomDeductions:0.00}</p>
                      <p><strong>Net Pay:</strong> {payrollLine.NetPay:0.00}</p>
                    </body>
                    </html>
                    """;

        var content = await _pdfRenderer.RenderAsync(html, cancellationToken);
        return new PayslipDocument($"{payrollLine.EmployeeName.Replace(" ", string.Empty)}-{payrollLine.PayrollRun?.RunNumber}.pdf", "application/pdf", content);
    }

    private Guid GetTenantId() =>
        _tenantContext.TenantId ?? throw new InvalidOperationException("Tenant context is required for payroll operations.");

    private static PayrollRunDto Map(PayrollRun payrollRun) =>
        new(
            payrollRun.Id,
            payrollRun.RunNumber,
            payrollRun.TenantId,
            payrollRun.CompanyId,
            payrollRun.PeriodStart,
            payrollRun.PeriodEnd,
            payrollRun.Cycle,
            payrollRun.Status,
            payrollRun.GrossTotal,
            payrollRun.TotalDeductions,
            payrollRun.NetTotal,
            payrollRun.ProcessedUtc,
            payrollRun.PayrollLines
                .OrderBy(x => x.EmployeeName)
                .Select(x => new PayrollLineDto(
                    x.Id,
                    x.EmployeeId,
                    x.EmployeeName,
                    x.GrossPay,
                    x.Paye,
                    x.UifEmployee,
                    x.CustomDeductions,
                    x.NetPay))
                .ToList());
}

