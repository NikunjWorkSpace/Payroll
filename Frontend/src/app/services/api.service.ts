import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { TenantService } from './tenant.service';

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(
    private readonly authService: AuthService,
    private readonly tenantService: TenantService
  ) {}

  get baseUrl(): string {
    return environment.apiBaseUrl;
  }

  createOptions(): { headers: HttpHeaders } {
    let headers = new HttpHeaders();

    const token = this.authService.token;
    const tenantId = this.tenantService.currentTenantId;

    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }

    if (tenantId) {
      headers = headers.set('X-Tenant-Id', tenantId);
    }

    return { headers };
  }
}

