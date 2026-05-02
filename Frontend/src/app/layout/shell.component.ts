import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Company } from '../models/company.model';
import { AuthService } from '../services/auth.service';
import { CompanyService } from '../services/company.service';
import { TenantService } from '../services/tenant.service';

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrls: ['./shell.component.css']
})
export class ShellComponent implements OnInit {
  companies: Company[] = [];

  constructor(
    private readonly authService: AuthService,
    private readonly companyService: CompanyService,
    private readonly tenantService: TenantService,
    private readonly router: Router
  ) {}

  get selectedTenantId(): string | null {
    return this.tenantService.currentTenantId;
  }

  get userRole(): string {
    return this.authService.session?.role ?? 'Guest';
  }

  get userName(): string {
    return this.authService.session?.fullName ?? 'PayBridge User';
  }

  ngOnInit(): void {
    if (!this.authService.session) {
      this.router.navigate(['/login']);
      return;
    }

    this.companyService.getCompanies().subscribe({
      next: (companies) => {
        this.companies = companies;
        if (!this.selectedTenantId && companies.length > 0) {
          this.tenantService.setTenant(companies[0].tenantId);
        }
      }
    });
  }

  onTenantChanged(tenantId: string): void {
    this.tenantService.setTenant(tenantId);
    this.router.navigate(['/dashboard']);
  }

  logout(): void {
    this.authService.logout();
    this.tenantService.setTenant(null);
    this.router.navigate(['/login']);
  }
}

