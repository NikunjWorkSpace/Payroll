namespace SA.Payroll.Application.Employees;

public interface IEmployeeService
{
    Task<IReadOnlyList<EmployeeDto>> GetByTenantAsync(CancellationToken cancellationToken = default);

    Task<EmployeeDto> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default);

    Task<EmployeeDto> UpdateLeaveBalanceAsync(Guid employeeId, UpdateLeaveBalanceRequest request, CancellationToken cancellationToken = default);
}

