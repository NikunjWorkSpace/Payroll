export interface PayrollLine {
  id: string;
  employeeId: string;
  employeeName: string;
  grossPay: number;
  paye: number;
  uifEmployee: number;
  customDeductions: number;
  netPay: number;
}

export interface PayrollRun {
  id: string;
  runNumber: string;
  tenantId: string;
  companyId: string;
  periodStart: string;
  periodEnd: string;
  cycle: string | number;
  status: string | number;
  grossTotal: number;
  totalDeductions: number;
  netTotal: number;
  processedUtc: string;
  payrollLines: PayrollLine[];
}

export interface PayrollSummary {
  activeEmployees: number;
  latestGrossTotal: number;
  latestDeductions: number;
  latestNetTotal: number;
  latestProcessedUtc?: string | null;
}
