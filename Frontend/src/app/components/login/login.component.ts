import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { TenantService } from '../../services/tenant.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  isSubmitting = false;
  errorMessage = '';

  form = this.formBuilder.group({
    email: ['global.admin@paybridge.co.za', [Validators.required, Validators.email]],
    password: ['Pass@123', Validators.required]
  });

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly tenantService: TenantService,
    private readonly router: Router
  ) {}

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    this.authService.login(this.form.getRawValue() as { email: string; password: string }).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.tenantService.setTenant(response.tenantId ?? null);
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Unable to sign in with the demo credentials.';
      }
    });
  }
}

