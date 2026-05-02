import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { environment } from '../../environments/environment';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  fullName: string;
  email: string;
  role: string;
  parentCompanyId?: string | null;
  tenantId?: string | null;
}

export interface UserSession extends AuthResponse {}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly storageKey = 'paybridge.session';

  constructor(private readonly http: HttpClient) {}

  get token(): string | null {
    return this.session?.accessToken ?? null;
  }

  get session(): UserSession | null {
    const raw = localStorage.getItem(this.storageKey);
    return raw ? (JSON.parse(raw) as UserSession) : null;
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${environment.apiBaseUrl}/auth/login`, request)
      .pipe(tap((response) => localStorage.setItem(this.storageKey, JSON.stringify(response))));
  }

  logout(): void {
    localStorage.removeItem(this.storageKey);
  }
}

