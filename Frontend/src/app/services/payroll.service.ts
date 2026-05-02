import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { PayrollRun, PayrollSummary } from '../models/payroll.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class PayrollService {
  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  getSummary(): Observable<PayrollSummary> {
    return this.http.get<PayrollSummary>(`${this.apiService.baseUrl}/payroll/summary`, this.apiService.createOptions());
  }

  getHistory(): Observable<PayrollRun[]> {
    return this.http.get<PayrollRun[]>(`${this.apiService.baseUrl}/payroll/history`, this.apiService.createOptions());
  }

  processPayroll(payload: Record<string, unknown>): Observable<PayrollRun> {
    return this.http.post<PayrollRun>(`${this.apiService.baseUrl}/payroll/process`, payload, this.apiService.createOptions());
  }

  downloadPayslip(payrollLineId: string): Observable<Blob> {
    return this.http.get(`${this.apiService.baseUrl}/reports/payslips/${payrollLineId}`, {
      ...this.apiService.createOptions(),
      responseType: 'blob'
    });
  }
}

