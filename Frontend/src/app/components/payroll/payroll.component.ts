import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { PayrollRun, PayrollSummary } from '../../models/payroll.model';
import { PayrollService } from '../../services/payroll.service';

@Component({
  selector: 'app-payroll',
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.css']
})
export class PayrollComponent implements OnInit {
  summary?: PayrollSummary;
  history: PayrollRun[] = [];

  form = this.formBuilder.group({
    periodStart: ['', Validators.required],
    periodEnd: ['', Validators.required],
    cycle: ['Monthly', Validators.required]
  });

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly payrollService: PayrollService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  processPayroll(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.payrollService.processPayroll(this.form.getRawValue() as Record<string, unknown>).subscribe({
      next: () => this.loadData()
    });
  }

  private loadData(): void {
    this.payrollService.getSummary().subscribe({
      next: (summary) => (this.summary = summary)
    });

    this.payrollService.getHistory().subscribe({
      next: (history) => (this.history = history)
    });
  }
}

