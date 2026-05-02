using Microsoft.EntityFrameworkCore;
using SA.Payroll.Core.Entities;

namespace SA.Payroll.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Company> Companies { get; }

    DbSet<Department> Departments { get; }

    DbSet<Employee> Employees { get; }

    DbSet<LeaveBalance> LeaveBalances { get; }

    DbSet<PayrollLine> PayrollLines { get; }

    DbSet<PayrollRun> PayrollRuns { get; }

    DbSet<SalaryComponent> SalaryComponents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

