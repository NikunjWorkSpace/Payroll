import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CompanyManagementComponent } from './components/company-management/company-management.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EmployeeManagementComponent } from './components/employee-management/employee-management.component';
import { LoginComponent } from './components/login/login.component';
import { PayrollComponent } from './components/payroll/payroll.component';
import { ReportsComponent } from './components/reports/reports.component';
import { ShellComponent } from './layout/shell.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'companies', component: CompanyManagementComponent },
      { path: 'employees', component: EmployeeManagementComponent },
      { path: 'payroll', component: PayrollComponent },
      { path: 'reports', component: ReportsComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}

