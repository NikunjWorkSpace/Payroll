using SA.Payroll.Core.Entities;

namespace SA.Payroll.Application.Payroll;

public interface IPayrollCalculator
{
    PayrollCalculationResult Calculate(Employee employee, Company company);
}

