# PayBridge ER Overview

```mermaid
erDiagram
    Companies ||--o{ Departments : contains
    Companies ||--o{ Employees : employs
    Companies ||--o{ PayrollRuns : owns
    Employees ||--|| LeaveBalances : has
    Employees ||--o{ SalaryComponents : receives
    Employees ||--o{ PayrollLines : appears_on
    PayrollRuns ||--o{ PayrollLines : contains
    AppUsers }o--|| Companies : operates
    AuditLogs }o--o| AppUsers : performed_by
```

The checked-in [ERDiagram.png](/c:/WorkSpace/Payroll/Payroll/Docs/ERDiagram.png) is a placeholder image so the repository shape matches the requested deliverables. Use this markdown as the editable source for generating a richer diagram later.
