import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ShellComponent } from './layout/shell.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { CompanyManagementComponent } from './components/company-management/company-management.component';
import { EmployeeManagementComponent } from './components/employee-management/employee-management.component';
import { PayrollComponent } from './components/payroll/payroll.component';
import { ReportsComponent } from './components/reports/reports.component';
import { LoginComponent } from './components/login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    ShellComponent,
    DashboardComponent,
    CompanyManagementComponent,
    EmployeeManagementComponent,
    PayrollComponent,
    ReportsComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}

