namespace SA.Payroll.Application.Departments;

public interface IDepartmentService
{
    Task<IReadOnlyList<DepartmentDto>> GetByTenantAsync(CancellationToken cancellationToken = default);
}
