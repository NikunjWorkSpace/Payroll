namespace SA.Payroll.Application.Employees;

public sealed record UpdateLeaveBalanceRequest(
    decimal AnnualLeaveDays,
    decimal SickLeaveDays,
    decimal UnpaidLeaveDays,
    decimal CarriedForwardDays);

