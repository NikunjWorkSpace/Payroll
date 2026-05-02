import { Component, OnInit } from '@angular/core';

import { Company } from '../../models/company.model';
import { PayrollSummary } from '../../models/payroll.model';
import { CompanyService } from '../../services/company.service';
import { PayrollService } from '../../services/payroll.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  companies: Company[] = [];
  summary?: PayrollSummary;

  constructor(
    private readonly companyService: CompanyService,
    private readonly payrollService: PayrollService
  ) {}

  ngOnInit(): void {
    this.companyService.getCompanies().subscribe({
      next: (companies) => (this.companies = companies)
    });

    this.payrollService.getSummary().subscribe({
      next: (summary) => (this.summary = summary)
    });
  }
}

