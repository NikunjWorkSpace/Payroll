SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID('dbo.PayrollLines', 'U') IS NOT NULL DROP TABLE dbo.PayrollLines;
IF OBJECT_ID('dbo.PayrollRuns', 'U') IS NOT NULL DROP TABLE dbo.PayrollRuns;
IF OBJECT_ID('dbo.SalaryComponents', 'U') IS NOT NULL DROP TABLE dbo.SalaryComponents;
IF OBJECT_ID('dbo.LeaveBalances', 'U') IS NOT NULL DROP TABLE dbo.LeaveBalances;
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL DROP TABLE dbo.Employees;
IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL DROP TABLE dbo.Departments;
IF OBJECT_ID('dbo.AppUsers', 'U') IS NOT NULL DROP TABLE dbo.AppUsers;
IF OBJECT_ID('dbo.Companies', 'U') IS NOT NULL DROP TABLE dbo.Companies;
IF OBJECT_ID('dbo.AuditLogs', 'U') IS NOT NULL DROP TABLE dbo.AuditLogs;
GO

CREATE TABLE dbo.Companies
(
    CompanyId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    ParentCompanyId UNIQUEIDENTIFIER NULL,
    RegisteredName NVARCHAR(200) NOT NULL,
    RegistrationNumber NVARCHAR(50) NOT NULL,
    CompanyType NVARCHAR(30) NOT NULL,
    DefaultPayrollCycle NVARCHAR(20) NOT NULL,
    ContactEmail NVARCHAR(150) NOT NULL,
    CountryCode CHAR(2) NOT NULL DEFAULT 'ZA',
    UifRate DECIMAL(5,4) NOT NULL DEFAULT 0.0100,
    SdlRate DECIMAL(5,4) NOT NULL DEFAULT 0.0100,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ModifiedUtc DATETIME2 NULL
);
GO

ALTER TABLE dbo.Companies
ADD CONSTRAINT FK_Companies_ParentCompany
FOREIGN KEY (ParentCompanyId) REFERENCES dbo.Companies(CompanyId);
GO

CREATE TABLE dbo.Departments
(
    DepartmentId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    CompanyId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(120) NOT NULL,
    CostCenter NVARCHAR(50) NULL,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ModifiedUtc DATETIME2 NULL,
    CONSTRAINT FK_Departments_Companies FOREIGN KEY (CompanyId) REFERENCES dbo.Companies(CompanyId)
);
GO

CREATE UNIQUE INDEX IX_Departments_Tenant_Name ON dbo.Departments(TenantId, Name);
GO

CREATE TABLE dbo.Employees
(
    EmployeeId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    CompanyId UNIQUEIDENTIFIER NOT NULL,
    DepartmentId UNIQUEIDENTIFIER NOT NULL,
    EmployeeNumber NVARCHAR(30) NOT NULL,
    FirstName NVARCHAR(80) NOT NULL,
    LastName NVARCHAR(80) NOT NULL,
    IdNumber NVARCHAR(30) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Position NVARCHAR(120) NOT NULL,
    HireDate DATE NOT NULL,
    TerminationDate DATE NULL,
    BasicSalary DECIMAL(18,2) NOT NULL,
    EmploymentStatus NVARCHAR(30) NOT NULL DEFAULT 'Active',
    BankName NVARCHAR(100) NOT NULL,
    AccountNumber NVARCHAR(30) NOT NULL,
    BranchCode NVARCHAR(20) NOT NULL,
    AccountType NVARCHAR(30) NOT NULL,
    TaxNumber NVARCHAR(30) NOT NULL,
    IncomeTaxReference NVARCHAR(30) NOT NULL,
    IsUifExempt BIT NOT NULL DEFAULT 0,
    IsSdlExempt BIT NOT NULL DEFAULT 0,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ModifiedUtc DATETIME2 NULL,
    CONSTRAINT FK_Employees_Companies FOREIGN KEY (CompanyId) REFERENCES dbo.Companies(CompanyId),
    CONSTRAINT FK_Employees_Departments FOREIGN KEY (DepartmentId) REFERENCES dbo.Departments(DepartmentId)
);
GO

CREATE UNIQUE INDEX IX_Employees_Tenant_EmployeeNumber ON dbo.Employees(TenantId, EmployeeNumber);
GO

CREATE TABLE dbo.LeaveBalances
(
    LeaveBalanceId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    EmployeeId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    AnnualLeaveDays DECIMAL(10,2) NOT NULL DEFAULT 15.00,
    SickLeaveDays DECIMAL(10,2) NOT NULL DEFAULT 10.00,
    UnpaidLeaveDays DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    CarriedForwardDays DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ModifiedUtc DATETIME2 NULL,
    CONSTRAINT FK_LeaveBalances_Employees FOREIGN KEY (EmployeeId) REFERENCES dbo.Employees(EmployeeId)
);
GO

CREATE TABLE dbo.SalaryComponents
(
    SalaryComponentId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    EmployeeId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(120) NOT NULL,
    ComponentType NVARCHAR(30) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    IsRecurring BIT NOT NULL DEFAULT 1,
    IsTaxable BIT NOT NULL DEFAULT 1,
    EffectiveFrom DATE NOT NULL,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ModifiedUtc DATETIME2 NULL,
    CONSTRAINT FK_SalaryComponents_Employees FOREIGN KEY (EmployeeId) REFERENCES dbo.Employees(EmployeeId)
);
GO

CREATE TABLE dbo.PayrollRuns
(
    PayrollRunId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    CompanyId UNIQUEIDENTIFIER NOT NULL,
    PeriodStart DATE NOT NULL,
    PeriodEnd DATE NOT NULL,
    RunNumber NVARCHAR(50) NOT NULL,
    Cycle NVARCHAR(20) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    GrossTotal DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    TotalDeductions DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    NetTotal DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    ProcessedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ModifiedUtc DATETIME2 NULL,
    CONSTRAINT FK_PayrollRuns_Companies FOREIGN KEY (CompanyId) REFERENCES dbo.Companies(CompanyId)
);
GO

CREATE TABLE dbo.PayrollLines
(
    PayrollLineId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    PayrollRunId UNIQUEIDENTIFIER NOT NULL,
    EmployeeId UNIQUEIDENTIFIER NOT NULL,
    EmployeeName NVARCHAR(160) NOT NULL,
    GrossPay DECIMAL(18,2) NOT NULL,
    TaxableEarnings DECIMAL(18,2) NOT NULL,
    PAYE DECIMAL(18,2) NOT NULL,
    UIFEmployee DECIMAL(18,2) NOT NULL,
    SDLEmployer DECIMAL(18,2) NOT NULL,
    CustomDeductions DECIMAL(18,2) NOT NULL,
    NetPay DECIMAL(18,2) NOT NULL,
    Notes NVARCHAR(4000) NULL,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ModifiedUtc DATETIME2 NULL,
    CONSTRAINT FK_PayrollLines_PayrollRuns FOREIGN KEY (PayrollRunId) REFERENCES dbo.PayrollRuns(PayrollRunId),
    CONSTRAINT FK_PayrollLines_Employees FOREIGN KEY (EmployeeId) REFERENCES dbo.Employees(EmployeeId)
);
GO

CREATE TABLE dbo.AppUsers
(
    UserId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NULL,
    ParentCompanyId UNIQUEIDENTIFIER NULL,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    RoleName NVARCHAR(40) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE UNIQUE INDEX IX_AppUsers_Email ON dbo.AppUsers(Email);
GO

CREATE TABLE dbo.AuditLogs
(
    AuditLogId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NULL,
    UserId UNIQUEIDENTIFIER NULL,
    EntityName NVARCHAR(100) NOT NULL,
    EntityId UNIQUEIDENTIFIER NULL,
    ActionName NVARCHAR(80) NOT NULL,
    OldValues NVARCHAR(MAX) NULL,
    NewValues NVARCHAR(MAX) NULL,
    PerformedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProcessPayrollForTenant
    @TenantId UNIQUEIDENTIFIER,
    @PeriodStart DATE,
    @PeriodEnd DATE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH RecurringAdjustments AS
    (
        SELECT
            sc.EmployeeId,
            SUM(CASE WHEN sc.ComponentType IN ('Allowance', 'Bonus') THEN sc.Amount ELSE 0 END) AS RecurringAdditions,
            SUM(CASE WHEN sc.ComponentType = 'Deduction' THEN sc.Amount ELSE 0 END) AS RecurringDeductions
        FROM dbo.SalaryComponents sc
        WHERE sc.TenantId = @TenantId
          AND sc.IsRecurring = 1
        GROUP BY sc.EmployeeId
    )
    SELECT
        e.EmployeeId,
        CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
        e.BasicSalary + ISNULL(ra.RecurringAdditions, 0) AS GrossPay,
        CAST(
            CASE
                WHEN (e.BasicSalary + ISNULL(ra.RecurringAdditions, 0)) * 12 <= 237100 THEN (((e.BasicSalary + ISNULL(ra.RecurringAdditions, 0)) * 12) * 0.18 - 17235) / 12
                WHEN (e.BasicSalary + ISNULL(ra.RecurringAdditions, 0)) * 12 <= 370500 THEN (42678 + (((e.BasicSalary + ISNULL(ra.RecurringAdditions, 0)) * 12) - 237100) * 0.26 - 17235) / 12
                ELSE (77362 + (((e.BasicSalary + ISNULL(ra.RecurringAdditions, 0)) * 12) - 370500) * 0.31 - 17235) / 12
            END AS DECIMAL(18,2)
        ) AS PAYEEstimate,
        CAST(CASE WHEN e.IsUifExempt = 1 THEN 0 ELSE IIF(e.BasicSalary + ISNULL(ra.RecurringAdditions, 0) > 17712, 177.12, (e.BasicSalary + ISNULL(ra.RecurringAdditions, 0)) * 0.01) END AS DECIMAL(18,2)) AS UIFEmployee,
        CAST(CASE WHEN e.IsSdlExempt = 1 THEN 0 ELSE (e.BasicSalary + ISNULL(ra.RecurringAdditions, 0)) * 0.01 END AS DECIMAL(18,2)) AS SDLEmployer,
        CAST(ISNULL(ra.RecurringDeductions, 0) AS DECIMAL(18,2)) AS CustomDeductions,
        @PeriodStart AS PeriodStart,
        @PeriodEnd AS PeriodEnd
    FROM dbo.Employees e
    LEFT JOIN RecurringAdjustments ra ON ra.EmployeeId = e.EmployeeId
    WHERE e.TenantId = @TenantId
      AND e.EmploymentStatus = 'Active';
END;
GO

