using SA.Payroll.Application.Common;

namespace SA.Payroll.Application.Payroll;

public interface IPayrollService
{
    Task<PayrollRunDto> ProcessAsync(ProcessPayrollRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PayrollRunDto>> GetHistoryAsync(CancellationToken cancellationToken = default);

    Task<PayrollSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default);

    Task<PayslipDocument> GetPayslipAsync(Guid payrollLineId, CancellationToken cancellationToken = default);
}

