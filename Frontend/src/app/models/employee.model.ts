export interface Employee {
  id: string;
  tenantId: string;
  companyId: string;
  departmentId: string;
  employeeNumber: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  idNumber: string;
  position: string;
  basicSalary: number;
  status: string | number;
  annualLeaveDays: number;
  sickLeaveDays: number;
  unpaidLeaveDays: number;
}

