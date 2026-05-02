using Microsoft.EntityFrameworkCore;
using SA.Payroll.Application.Abstractions;
using SA.Payroll.Core.Entities;
using SA.Payroll.Core.ValueObjects;

namespace SA.Payroll.Application.Employees;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ITenantContext _tenantContext;

    public EmployeeService(IApplicationDbContext dbContext, ITenantContext tenantContext)
    {
        _dbContext = dbContext;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<EmployeeDto>> GetByTenantAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = GetTenantId();

        var employees = await _dbContext.Employees
            .AsNoTracking()
            .Include(x => x.LeaveBalance)
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName)
            .ToListAsync(cancellationToken);

        return employees.Select(Map).ToList();
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var tenantId = GetTenantId();

        var employeeId = Guid.NewGuid();
        var employee = new Employee
        {
            Id = employeeId,
            TenantId = tenantId,
            CompanyId = request.CompanyId,
            DepartmentId = request.DepartmentId,
            EmployeeNumber = request.EmployeeNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IdNumber = request.IdNumber,
            Email = request.Email,
            Position = request.Position,
            HireDate = request.HireDate,
            BasicSalary = request.BasicSalary,
            BankAccount = new BankAccountDetails(
                request.BankName,
                request.AccountNumber,
                request.BranchCode,
                request.AccountType),
            TaxProfile = new TaxProfile(
                request.TaxNumber,
                request.IncomeTaxReference,
                request.IsUifExempt,
                request.IsSdlExempt),
            LeaveBalance = new LeaveBalance
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                EmployeeId = employeeId
            },
            SalaryComponents = new List<SalaryComponent>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    EmployeeId = employeeId,
                    Name = "Basic Salary",
                    Type = SA.Payroll.Core.Enums.SalaryComponentType.Basic,
                    Amount = request.BasicSalary,
                    IsRecurring = true,
                    IsTaxable = true
                }
            }
        };

        _dbContext.Employees.Add(employee);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Map(employee);
    }

    public async Task<EmployeeDto> UpdateLeaveBalanceAsync(Guid employeeId, UpdateLeaveBalanceRequest request, CancellationToken cancellationToken = default)
    {
        var tenantId = GetTenantId();

        var employee = await _dbContext.Employees
            .Include(x => x.LeaveBalance)
            .FirstOrDefaultAsync(x => x.Id == employeeId && x.TenantId == tenantId, cancellationToken)
            ?? throw new KeyNotFoundException("Employee was not found for the selected tenant.");

        employee.LeaveBalance ??= new LeaveBalance
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            EmployeeId = employeeId
        };

        employee.LeaveBalance.AnnualLeaveDays = request.AnnualLeaveDays;
        employee.LeaveBalance.SickLeaveDays = request.SickLeaveDays;
        employee.LeaveBalance.UnpaidLeaveDays = request.UnpaidLeaveDays;
        employee.LeaveBalance.CarriedForwardDays = request.CarriedForwardDays;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Map(employee);
    }

    private Guid GetTenantId() =>
        _tenantContext.TenantId ?? throw new InvalidOperationException("Tenant context is required for employee operations.");

    private static EmployeeDto Map(Employee employee) =>
        new(
            employee.Id,
            employee.TenantId,
            employee.CompanyId,
            employee.DepartmentId,
            employee.EmployeeNumber,
            employee.FirstName,
            employee.LastName,
            employee.FullName,
            employee.Email,
            employee.IdNumber,
            employee.Position,
            employee.BasicSalary,
            employee.Status,
            employee.LeaveBalance?.AnnualLeaveDays ?? 0,
            employee.LeaveBalance?.SickLeaveDays ?? 0,
            employee.LeaveBalance?.UnpaidLeaveDays ?? 0);
}
