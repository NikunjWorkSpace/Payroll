namespace SA.Payroll.Application.Payroll;

public sealed record PayrollLineDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    decimal GrossPay,
    decimal Paye,
    decimal UifEmployee,
    decimal CustomDeductions,
    decimal NetPay);

