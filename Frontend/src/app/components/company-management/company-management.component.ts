import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { Company } from '../../models/company.model';
import { CompanyService } from '../../services/company.service';

@Component({
  selector: 'app-company-management',
  templateUrl: './company-management.component.html',
  styleUrls: ['./company-management.component.css']
})
export class CompanyManagementComponent implements OnInit {
  companies: Company[] = [];

  form = this.formBuilder.group({
    registeredName: ['', Validators.required],
    registrationNumber: ['', Validators.required],
    contactEmail: ['', [Validators.required, Validators.email]],
    parentCompanyId: [''],
    companyType: ['Subsidiary', Validators.required],
    defaultPayrollCycle: ['Monthly', Validators.required],
    uifRate: [0.01, Validators.required],
    sdlRate: [0.01, Validators.required]
  });

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly companyService: CompanyService
  ) {}

  ngOnInit(): void {
    this.loadCompanies();
  }

  createCompany(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.companyService.createCompany(this.form.getRawValue()).subscribe({
      next: () => {
        this.form.reset({
          companyType: 'Subsidiary',
          defaultPayrollCycle: 'Monthly',
          uifRate: 0.01,
          sdlRate: 0.01
        });
        this.loadCompanies();
      }
    });
  }

  private loadCompanies(): void {
    this.companyService.getCompanies().subscribe({
      next: (companies) => (this.companies = companies)
    });
  }
}

