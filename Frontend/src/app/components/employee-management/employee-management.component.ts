import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { Department } from '../../models/department.model';
import { Employee } from '../../models/employee.model';
import { DepartmentService } from '../../services/department.service';
import { EmployeeService } from '../../services/employee.service';
import { TenantService } from '../../services/tenant.service';

@Component({
  selector: 'app-employee-management',
  templateUrl: './employee-management.component.html',
  styleUrls: ['./employee-management.component.css']
})
export class EmployeeManagementComponent implements OnInit {
  departments: Department[] = [];
  employees: Employee[] = [];

  form = this.formBuilder.group({
    companyId: ['', Validators.required],
    departmentId: ['', Validators.required],
    employeeNumber: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    idNumber: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    position: ['', Validators.required],
    hireDate: ['', Validators.required],
    basicSalary: [0, Validators.required],
    bankName: ['FNB', Validators.required],
    accountNumber: ['', Validators.required],
    branchCode: ['250655', Validators.required],
    accountType: ['Cheque', Validators.required],
    taxNumber: ['', Validators.required],
    incomeTaxReference: ['', Validators.required],
    isUifExempt: [false],
    isSdlExempt: [false]
  });

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly departmentService: DepartmentService,
    private readonly employeeService: EmployeeService,
    private readonly tenantService: TenantService
  ) {}

  ngOnInit(): void {
    const tenantId = this.tenantService.currentTenantId;
    if (tenantId) {
      this.form.patchValue({ companyId: tenantId });
    }

    this.loadEmployees();
    this.loadDepartments();
  }

  createEmployee(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.employeeService.createEmployee(this.form.getRawValue() as Record<string, unknown>).subscribe({
      next: () => this.loadEmployees()
    });
  }

  private loadEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: (employees) => (this.employees = employees)
    });
  }

  private loadDepartments(): void {
    this.departmentService.getDepartments().subscribe({
      next: (departments) => {
        this.departments = departments;
        if (!this.form.value.departmentId && departments.length > 0) {
          this.form.patchValue({ departmentId: departments[0].id });
        }
      }
    });
  }
}
