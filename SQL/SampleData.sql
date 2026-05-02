SET NOCOUNT ON;
GO

INSERT INTO dbo.Companies
(
    CompanyId,
    TenantId,
    ParentCompanyId,
    RegisteredName,
    RegistrationNumber,
    CompanyType,
    DefaultPayrollCycle,
    ContactEmail
)
VALUES
('11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111', NULL, 'PayBridge Holdings', '2018/100001/07', 'Parent', 'Monthly', 'group.finance@paybridge.co.za'),
('22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222222', '11111111-1111-1111-1111-111111111111', 'PayBridge Manufacturing', '2020/200002/07', 'Subsidiary', 'Monthly', 'payroll.manufacturing@paybridge.co.za'),
('33333333-3333-3333-3333-333333333333', '33333333-3333-3333-3333-333333333333', '11111111-1111-1111-1111-111111111111', 'PayBridge Services', '2021/300003/07', 'Subsidiary', 'Monthly', 'payroll.services@paybridge.co.za');
GO

INSERT INTO dbo.Departments
(DepartmentId, TenantId, CompanyId, Name, CostCenter)
VALUES
('44444444-4444-4444-4444-444444444444', '22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222222', 'Finance', 'FIN-100'),
('55555555-5555-5555-5555-555555555555', '22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222222', 'Operations', 'OPS-200'),
('66666666-6666-6666-6666-666666666666', '33333333-3333-3333-3333-333333333333', '33333333-3333-3333-3333-333333333333', 'Consulting', 'CON-300');
GO

INSERT INTO dbo.Employees
(
    EmployeeId,
    TenantId,
    CompanyId,
    DepartmentId,
    EmployeeNumber,
    FirstName,
    LastName,
    IdNumber,
    Email,
    Position,
    HireDate,
    BasicSalary,
    BankName,
    AccountNumber,
    BranchCode,
    AccountType,
    TaxNumber,
    IncomeTaxReference
)
VALUES
('77777777-7777-7777-7777-777777777777', '22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222222', '44444444-4444-4444-4444-444444444444', 'EMP-1001', 'Lerato', 'Nkosi', '9001015009087', 'lerato.nkosi@paybridge.co.za', 'Payroll Officer', '2023-02-01', 35000.00, 'FNB', '1234567890', '250655', 'Cheque', '9988776655', '1234567890'),
('88888888-8888-8888-8888-888888888888', '22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222222', '55555555-5555-5555-5555-555555555555', 'EMP-1002', 'Thabo', 'Mokoena', '8805055109080', 'thabo.mokoena@paybridge.co.za', 'Operations Supervisor', '2022-08-15', 28000.00, 'Nedbank', '1122334455', '198765', 'Savings', '8877665544', '9876543210');
GO

INSERT INTO dbo.LeaveBalances
(LeaveBalanceId, TenantId, EmployeeId, AnnualLeaveDays, SickLeaveDays, UnpaidLeaveDays, CarriedForwardDays)
VALUES
('99999999-9999-9999-9999-999999999999', '22222222-2222-2222-2222-222222222222', '77777777-7777-7777-7777-777777777777', 14.50, 8.00, 0.00, 2.00),
('aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee', '22222222-2222-2222-2222-222222222222', '88888888-8888-8888-8888-888888888888', 12.00, 9.00, 0.00, 1.00);
GO

INSERT INTO dbo.SalaryComponents
(SalaryComponentId, TenantId, EmployeeId, Name, ComponentType, Amount, IsRecurring, IsTaxable, EffectiveFrom)
VALUES
('abababab-abab-abab-abab-abababababab', '22222222-2222-2222-2222-222222222222', '77777777-7777-7777-7777-777777777777', 'Travel Allowance', 'Allowance', 2500.00, 1, 1, '2024-01-01'),
('bcbcbcbc-bcbc-bcbc-bcbc-bcbcbcbcbcbc', '22222222-2222-2222-2222-222222222222', '77777777-7777-7777-7777-777777777777', 'Medical Aid Top-up', 'Deduction', 850.00, 1, 0, '2024-01-01'),
('cdcdcdcd-cdcd-cdcd-cdcd-cdcdcdcdcdcd', '22222222-2222-2222-2222-222222222222', '88888888-8888-8888-8888-888888888888', 'Shift Allowance', 'Allowance', 1800.00, 1, 1, '2024-01-01');
GO

INSERT INTO dbo.PayrollRuns
(
    PayrollRunId,
    TenantId,
    CompanyId,
    PeriodStart,
    PeriodEnd,
    RunNumber,
    Cycle,
    Status,
    GrossTotal,
    TotalDeductions,
    NetTotal,
    ProcessedUtc
)
VALUES
('dededede-dede-dede-dede-dededededede', '22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222222', '2026-02-01', '2026-02-28', 'PB-202602-090000', 'Monthly', 'Processed', 67300.00, 15422.12, 51877.88, '2026-02-28T09:00:00');
GO

INSERT INTO dbo.PayrollLines
(
    PayrollLineId,
    TenantId,
    PayrollRunId,
    EmployeeId,
    EmployeeName,
    GrossPay,
    TaxableEarnings,
    PAYE,
    UIFEmployee,
    SDLEmployer,
    CustomDeductions,
    NetPay,
    Notes
)
VALUES
('efefefef-efef-efef-efef-efefefefefef', '22222222-2222-2222-2222-222222222222', 'dededede-dede-dede-dede-dededededede', '77777777-7777-7777-7777-777777777777', 'Lerato Nkosi', 37500.00, 37500.00, 9500.00, 177.12, 375.00, 850.00, 26972.88, 'Travel allowance included'),
('f0f0f0f0-f0f0-f0f0-f0f0-f0f0f0f0f0f0', '22222222-2222-2222-2222-222222222222', 'dededede-dede-dede-dede-dededededede', '88888888-8888-8888-8888-888888888888', 'Thabo Mokoena', 29800.00, 29800.00, 4700.00, 177.12, 298.00, 0.00, 24922.88, 'Shift allowance included');
GO

INSERT INTO dbo.AppUsers
(UserId, TenantId, ParentCompanyId, FullName, Email, PasswordHash, RoleName)
VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NULL, '11111111-1111-1111-1111-111111111111', 'Global Administrator', 'global.admin@paybridge.co.za', 'Pass@123', 'GlobalAdmin'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', '22222222-2222-2222-2222-222222222222', '11111111-1111-1111-1111-111111111111', 'Company Administrator', 'company.admin@paybridge.co.za', 'Pass@123', 'CompanyAdmin'),
('cccccccc-cccc-cccc-cccc-cccccccccccc', '22222222-2222-2222-2222-222222222222', '11111111-1111-1111-1111-111111111111', 'Payroll Officer', 'payroll.officer@paybridge.co.za', 'Pass@123', 'PayrollOfficer'),
('dddddddd-dddd-dddd-dddd-dddddddddddd', '22222222-2222-2222-2222-222222222222', '11111111-1111-1111-1111-111111111111', 'Employee User', 'employee@paybridge.co.za', 'Pass@123', 'Employee');
GO

