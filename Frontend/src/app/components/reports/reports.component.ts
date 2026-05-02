import { Component, OnInit } from '@angular/core';

import { PayrollRun } from '../../models/payroll.model';
import { PayrollService } from '../../services/payroll.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  history: PayrollRun[] = [];

  constructor(private readonly payrollService: PayrollService) {}

  ngOnInit(): void {
    this.payrollService.getHistory().subscribe({
      next: (history) => (this.history = history)
    });
  }

  downloadPayslip(payrollLineId: string): void {
    this.payrollService.downloadPayslip(payrollLineId).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const anchor = document.createElement('a');
        anchor.href = url;
        anchor.download = 'payslip.pdf';
        anchor.click();
        window.URL.revokeObjectURL(url);
      }
    });
  }
}

