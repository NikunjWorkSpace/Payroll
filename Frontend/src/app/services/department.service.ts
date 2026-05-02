import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Department } from '../models/department.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class DepartmentService {
  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  getDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(`${this.apiService.baseUrl}/departments`, this.apiService.createOptions());
  }
}

