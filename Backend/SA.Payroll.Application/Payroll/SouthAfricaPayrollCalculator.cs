using SA.Payroll.Core.Entities;
using SA.Payroll.Core.Enums;

namespace SA.Payroll.Application.Payroll;

public sealed class SouthAfricaPayrollCalculator : IPayrollCalculator
{
    private const decimal MonthlyUifRemunerationCap = 17712m;

    private const decimal PrimaryRebateSample2024_2025 = 17235m;

    private static readonly IReadOnlyList<TaxBracket> AnnualTaxBracketsSample2024_2025 =
        new List<TaxBracket>
        {
            new(0m, 0m, 0.18m),
            new(237100m, 42678m, 0.26m),
            new(370500m, 77362m, 0.31m),
            new(512800m, 121475m, 0.36m),
            new(673000m, 179147m, 0.39m),
            new(857900m, 251258m, 0.41m),
            new(1817000m, 644489m, 0.45m)
        };

    public PayrollCalculationResult Calculate(Employee employee, Company company)
    {
        var additions = employee.SalaryComponents
            .Where(x => x.IsRecurring && (x.Type is SalaryComponentType.Allowance or SalaryComponentType.Bonus))
            .Sum(x => x.Amount);

        var taxableAdditions = employee.SalaryComponents
            .Where(x => x.IsRecurring && x.IsTaxable && (x.Type is SalaryComponentType.Allowance or SalaryComponentType.Bonus))
            .Sum(x => x.Amount);

        var customDeductions = employee.SalaryComponents
            .Where(x => x.IsRecurring && x.Type == SalaryComponentType.Deduction)
            .Sum(x => x.Amount);

        var grossPay = employee.BasicSalary + additions;
        var taxableMonthly = employee.BasicSalary + taxableAdditions;
        var annualTaxable = taxableMonthly * 12m;
        var annualTaxBeforeRebate = CalculateAnnualTax(annualTaxable);
        var paye = Math.Round(
            Math.Max((annualTaxBeforeRebate - PrimaryRebateSample2024_2025) / 12m, 0m),
            2,
            MidpointRounding.AwayFromZero);

        var uifEmployee = employee.TaxProfile.IsUifExempt
            ? 0m
            : Math.Round(
                Math.Min(grossPay, MonthlyUifRemunerationCap) * company.UifRate,
                2,
                MidpointRounding.AwayFromZero);

        var sdlEmployer = employee.TaxProfile.IsSdlExempt
            ? 0m
            : Math.Round(grossPay * company.SdlRate, 2, MidpointRounding.AwayFromZero);

        var netPay = Math.Round(grossPay - paye - uifEmployee - customDeductions, 2, MidpointRounding.AwayFromZero);

        var breakdown = new Dictionary<string, decimal>
        {
            ["BasicSalary"] = employee.BasicSalary,
            ["RecurringAdditions"] = additions,
            ["Paye"] = paye,
            ["UifEmployee"] = uifEmployee,
            ["SdlEmployer"] = sdlEmployer,
            ["CustomDeductions"] = customDeductions,
            ["NetPay"] = netPay
        };

        return new PayrollCalculationResult(
            grossPay,
            taxableMonthly,
            paye,
            uifEmployee,
            sdlEmployer,
            customDeductions,
            netPay,
            breakdown);
    }

    private static decimal CalculateAnnualTax(decimal annualTaxable)
    {
        var bracket = AnnualTaxBracketsSample2024_2025
            .Where(x => annualTaxable >= x.Threshold)
            .MaxBy(x => x.Threshold)
            ?? AnnualTaxBracketsSample2024_2025[0];

        return bracket.BaseTax + ((annualTaxable - bracket.Threshold) * bracket.Rate);
    }
}
