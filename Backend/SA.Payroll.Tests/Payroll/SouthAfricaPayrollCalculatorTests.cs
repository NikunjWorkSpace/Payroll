using SA.Payroll.Application.Payroll;
using SA.Payroll.Core.Entities;
using SA.Payroll.Core.Enums;
using SA.Payroll.Core.ValueObjects;

namespace SA.Payroll.Tests.Payroll;

public sealed class SouthAfricaPayrollCalculatorTests
{
    private readonly SouthAfricaPayrollCalculator _calculator = new();

    [Fact]
    public void Calculate_ShouldIncludePayeUifAndNetPay()
    {
        var employee = BuildEmployee(35000m);
        employee.SalaryComponents.Add(new SalaryComponent
        {
            TenantId = employee.TenantId,
            EmployeeId = employee.Id,
            Name = "Travel Allowance",
            Type = SalaryComponentType.Allowance,
            Amount = 2500m,
            IsRecurring = true,
            IsTaxable = true
        });

        var company = BuildCompany();

        var result = _calculator.Calculate(employee, company);

        Assert.Equal(37500m, result.GrossPay);
        Assert.True(result.Paye > 0m);
        Assert.Equal(177.12m, result.UifEmployee);
        Assert.True(result.NetPay < result.GrossPay);
    }

    [Fact]
    public void Calculate_ShouldRespectUifExemption()
    {
        var employee = BuildEmployee(18000m);
        employee.TaxProfile = employee.TaxProfile with { IsUifExempt = true };
        var company = BuildCompany();

        var result = _calculator.Calculate(employee, company);

        Assert.Equal(0m, result.UifEmployee);
    }

    [Fact]
    public void Calculate_ShouldIncludeCustomRecurringDeductions()
    {
        var employee = BuildEmployee(22000m);
        employee.SalaryComponents.Add(new SalaryComponent
        {
            TenantId = employee.TenantId,
            EmployeeId = employee.Id,
            Name = "Loan Repayment",
            Type = SalaryComponentType.Deduction,
            Amount = 1250m,
            IsRecurring = true,
            IsTaxable = false
        });

        var company = BuildCompany();

        var result = _calculator.Calculate(employee, company);

        Assert.Equal(1250m, result.CustomDeductions);
        Assert.Equal(result.GrossPay - result.Paye - result.UifEmployee - 1250m, result.NetPay);
    }

    private static Company BuildCompany() =>
        new()
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            RegisteredName = "PayBridge Demo",
            RegistrationNumber = "2024/000001/07",
            CompanyType = CompanyType.Subsidiary,
            DefaultPayrollCycle = PayrollCycle.Monthly,
            ContactEmail = "finance@paybridge.co.za",
            UifRate = 0.01m,
            SdlRate = 0.01m
        };

    private static Employee BuildEmployee(decimal basicSalary) =>
        new()
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            CompanyId = Guid.NewGuid(),
            DepartmentId = Guid.NewGuid(),
            EmployeeNumber = "EMP-001",
            FirstName = "Lerato",
            LastName = "Nkosi",
            IdNumber = "9001015009087",
            Email = "lerato.nkosi@paybridge.co.za",
            Position = "Payroll Specialist",
            BasicSalary = basicSalary,
            BankAccount = new BankAccountDetails("FNB", "1234567890", "250655", "Cheque"),
            TaxProfile = new TaxProfile("9999999999", "1234567890", false, false)
        };
}
