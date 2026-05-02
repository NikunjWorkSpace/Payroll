import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Employee } from '../models/employee.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.apiService.baseUrl}/employees`, this.apiService.createOptions());
  }

  createEmployee(payload: Record<string, unknown>): Observable<Employee> {
    return this.http.post<Employee>(`${this.apiService.baseUrl}/employees`, payload, this.apiService.createOptions());
  }
}

