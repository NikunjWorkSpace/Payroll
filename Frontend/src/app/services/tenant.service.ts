import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TenantService {
  private readonly storageKey = 'paybridge.tenant';

  get currentTenantId(): string | null {
    return localStorage.getItem(this.storageKey);
  }

  setTenant(tenantId: string | null): void {
    if (!tenantId) {
      localStorage.removeItem(this.storageKey);
      return;
    }

    localStorage.setItem(this.storageKey, tenantId);
  }
}

