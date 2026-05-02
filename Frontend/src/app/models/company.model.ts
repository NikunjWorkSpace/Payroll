export interface Company {
  id: string;
  tenantId: string;
  parentCompanyId?: string | null;
  registeredName: string;
  registrationNumber: string;
  companyType: 'Parent' | 'Subsidiary' | number;
  defaultPayrollCycle: 'Monthly' | 'AdHoc' | number;
  contactEmail: string;
  isActive: boolean;
}

