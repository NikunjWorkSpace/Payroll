# PayBridge

PayBridge is a multi-tenant payroll platform for South African businesses. It is designed for parent companies that manage multiple subsidiaries with independent employees, payroll runs, leave balances, statutory deductions, and reporting requirements.

## What is included

- `.NET 7` Web API scaffold using Clean Architecture
- `Angular 17` frontend scaffold with tenant-aware navigation
- `SQL Server` schema and sample data scripts
- JWT authentication flow with demo users
- Payroll calculation service for PAYE, UIF, SDL, and recurring deductions
- Sample payslip and SARS-style export artifact
- Unit tests for payroll calculation logic

## Solution structure

```text
PayBridge/
├── Backend/
│   ├── SA.Payroll.Core/
│   ├── SA.Payroll.Application/
│   ├── SA.Payroll.Infrastructure/
│   ├── SA.Payroll.API/
│   └── SA.Payroll.Tests/
├── Frontend/
├── SQL/
├── Docs/
└── README.md
```

## Architecture

### Backend

- `SA.Payroll.Core`: entities, enums, and value objects
- `SA.Payroll.Application`: business services, tenant-aware use cases, payroll rules, and contracts
- `SA.Payroll.Infrastructure`: EF Core persistence, JWT token generation, demo identity provider, and stub PDF rendering
- `SA.Payroll.API`: controllers, middleware, auth configuration, Swagger, and request pipeline

### Frontend

- Angular 17 module-based shell
- Login, Dashboard, Company Management, Employee Management, Payroll, and Reports screens
- Shared services for auth, tenant selection, and API access
- Responsive layout designed for subsidiary switching and payroll operations
- Repo-ready as its own standalone project under [Frontend](/c:/WorkSpace/Payroll/Payroll/Frontend)

### Data model

Core tables include:

- `Companies`
- `Departments`
- `Employees`
- `LeaveBalances`
- `SalaryComponents`
- `PayrollRuns`
- `PayrollLines`
- `AppUsers`
- `AuditLogs`

All payroll-sensitive tables carry a `TenantId` for tenant isolation.

## Key backend features

- Multi-tenant context resolution through `X-Tenant-Id` header or JWT claims
- Role-aware access for `GlobalAdmin`, `CompanyAdmin`, `PayrollOfficer`, and `Employee`
- South African payroll calculation service using sample SARS `2024/2025` tax bracket logic
- Leave balance tracking per employee
- Payslip generation endpoint returning a starter PDF payload stub
- Swagger/OpenAPI enabled in development

## Demo users

These credentials are configured in [Backend/SA.Payroll.API/appsettings.json](/c:/WorkSpace/Payroll/Payroll/Backend/SA.Payroll.API/appsettings.json):

- `global.admin@paybridge.co.za` / `Pass@123`
- `company.admin@paybridge.co.za` / `Pass@123`
- `payroll.officer@paybridge.co.za` / `Pass@123`
- `employee@paybridge.co.za` / `Pass@123`

## API surface

Core endpoints:

- `POST /api/auth/login`
- `GET /api/companies`
- `POST /api/companies`
- `GET /api/departments`
- `GET /api/employees`
- `POST /api/employees`
- `PUT /api/employees/{employeeId}/leave-balance`
- `GET /api/payroll/summary`
- `GET /api/payroll/history`
- `POST /api/payroll/process`
- `GET /api/reports/statutory-summary`
- `GET /api/reports/payslips/{payrollLineId}`

## Getting started

### Prerequisites

- .NET 7 SDK
- Node.js 20+ and npm
- Angular CLI 17
- SQL Server 2019+ or Azure SQL
- Docker Desktop or Docker Engine for containerized startup

### Database

1. Create a SQL Server database named `PayBridgeDb`.
2. Run [SQL/CreateTables.sql](/c:/WorkSpace/Payroll/Payroll/SQL/CreateTables.sql).
3. Run [SQL/SampleData.sql](/c:/WorkSpace/Payroll/Payroll/SQL/SampleData.sql).

### Backend

1. Update the connection string in [Backend/SA.Payroll.API/appsettings.json](/c:/WorkSpace/Payroll/Payroll/Backend/SA.Payroll.API/appsettings.json).
2. Restore and run:

```powershell
cd Backend
dotnet restore
dotnet build
$env:ASPNETCORE_URLS="http://localhost:8080"
dotnet run --project SA.Payroll.API
```

3. Open Swagger at `http://localhost:8080/swagger`.

### Frontend

1. Install dependencies:

```powershell
cd Frontend
npm install
```

2. Start the Angular app:

```powershell
npm start
```

3. Open `http://localhost:4200`.

## Testing

Run unit tests from the `Backend` folder:

```powershell
dotnet test
```

## Docker quick start

The repository now supports a full local stack with SQL Server, automatic database bootstrap, the API, and the Angular app.

### What Docker starts

- `sqlserver`: SQL Server 2022 Developer Edition
- `db-init`: creates `PayBridgeDb` and applies [SQL/Bootstrap.sql](/c:/WorkSpace/Payroll/Payroll/SQL/Bootstrap.sql), [SQL/CreateTables.sql](/c:/WorkSpace/Payroll/Payroll/SQL/CreateTables.sql), and [SQL/SampleData.sql](/c:/WorkSpace/Payroll/Payroll/SQL/SampleData.sql)
- `api`: PayBridge `.NET 7` Web API on `http://localhost:8080`
- `web`: Angular 17 app on `http://localhost:4200`

### Run the full stack

1. Optionally copy [.env.example](/c:/WorkSpace/Payroll/Payroll/.env.example) to `.env` and change the SQL Server password.
2. Start the stack:

```powershell
docker compose up --build
```

### Stop the stack

```powershell
docker compose down
```

### Reset the SQL Server volume

```powershell
docker compose down -v
```

### Container notes

- The frontend is configured to call the API at `http://localhost:8080/api`.
- The frontend Docker build now uses `Frontend/` as an independent build context so it can be moved into its own repository cleanly.
- The API uses a container-specific SQL connection string from [docker-compose.yml](/c:/WorkSpace/Payroll/Payroll/docker-compose.yml).
- The SQL Server `sa` password can be overridden with a local `.env` file based on [.env.example](/c:/WorkSpace/Payroll/Payroll/.env.example).
- HTTPS redirection is disabled in development containers so local Docker traffic stays on plain HTTP.
- SQL bootstrap is repeatable because the schema script recreates the tables each time the database is initialized.

## SQL assets and reporting samples

- Schema: [SQL/CreateTables.sql](/c:/WorkSpace/Payroll/Payroll/SQL/CreateTables.sql)
- Docker bootstrap: [SQL/Bootstrap.sql](/c:/WorkSpace/Payroll/Payroll/SQL/Bootstrap.sql)
- Seed data: [SQL/SampleData.sql](/c:/WorkSpace/Payroll/Payroll/SQL/SampleData.sql)
- Sample payslip: [Docs/SamplePayslip.html](/c:/WorkSpace/Payroll/Payroll/Docs/SamplePayslip.html)
- Sample SARS summary: [Docs/SARS_EMP201_Sample.csv](/c:/WorkSpace/Payroll/Payroll/Docs/SARS_EMP201_Sample.csv)

## Important implementation notes

- The PAYE calculator currently uses a sample `2024/2025` SARS tax table shape in code. Update the brackets and rebates before production use in future filing periods.
- The PDF renderer is intentionally a starter stub that returns HTML bytes. Replace it with a real PDF engine such as QuestPDF, DinkToPdf, or an external document service.
- Authentication is seeded with demo users for local development. Replace this with ASP.NET Identity, Entra ID, Auth0, or another production-grade identity provider.
- The frontend is scaffolded for Angular 17, but dependency installation is still required before it can be built.

## Suggested next steps

1. Add department create/update flows and richer org-structure management.
2. Replace the demo identity store with persistent users and password hashing.
3. Introduce proper PDF generation and email payslip delivery.
4. Add tenant-aware audit logging interception and background jobs.
5. Expand statutory reporting for EMP201, IRP5, and leave compliance workflows.
