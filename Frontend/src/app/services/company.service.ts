import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Company } from '../models/company.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class CompanyService {
  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  getCompanies(): Observable<Company[]> {
    return this.http.get<Company[]>(`${this.apiService.baseUrl}/companies`, this.apiService.createOptions());
  }

  createCompany(payload: Partial<Company>): Observable<Company> {
    return this.http.post<Company>(`${this.apiService.baseUrl}/companies`, payload, this.apiService.createOptions());
  }
}

