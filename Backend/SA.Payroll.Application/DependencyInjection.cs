using Microsoft.Extensions.DependencyInjection;
using SA.Payroll.Application.Companies;
using SA.Payroll.Application.Departments;
using SA.Payroll.Application.Employees;
using SA.Payroll.Application.Payroll;

namespace SA.Payroll.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IPayrollCalculator, SouthAfricaPayrollCalculator>();
        services.AddScoped<IPayrollService, PayrollService>();

        return services;
    }
}
