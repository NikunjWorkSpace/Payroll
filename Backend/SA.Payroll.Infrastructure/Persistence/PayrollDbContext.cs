using Microsoft.EntityFrameworkCore;
using SA.Payroll.Application.Abstractions;
using SA.Payroll.Core.Common;
using SA.Payroll.Core.Entities;

namespace SA.Payroll.Infrastructure.Persistence;

public sealed class PayrollDbContext : DbContext, IApplicationDbContext
{
    public PayrollDbContext(DbContextOptions<PayrollDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<LeaveBalance> LeaveBalances => Set<LeaveBalance>();

    public DbSet<PayrollLine> PayrollLines => Set<PayrollLine>();

    public DbSet<PayrollRun> PayrollRuns => Set<PayrollRun>();

    public DbSet<SalaryComponent> SalaryComponents => Set<SalaryComponent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.RegisteredName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.RegistrationNumber).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ContactEmail).HasMaxLength(150).IsRequired();
            entity.HasMany(x => x.Departments)
                .WithOne(x => x.Company)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.Employees)
                .WithOne(x => x.Company)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(x => x.PayrollRuns)
                .WithOne(x => x.Company)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Departments");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.CostCenter).HasMaxLength(50);
            entity.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.EmployeeNumber).HasMaxLength(30).IsRequired();
            entity.Property(x => x.FirstName).HasMaxLength(80).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(80).IsRequired();
            entity.Property(x => x.IdNumber).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Position).HasMaxLength(120).IsRequired();
            entity.Property(x => x.BasicSalary).HasColumnType("decimal(18,2)");
            entity.HasIndex(x => new { x.TenantId, x.EmployeeNumber }).IsUnique();
            entity.HasOne(x => x.Department)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.LeaveBalance)
                .WithOne(x => x.Employee)
                .HasForeignKey<LeaveBalance>(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.OwnsOne(x => x.BankAccount, owned =>
            {
                owned.Property(x => x.BankName).HasColumnName("BankName").HasMaxLength(100);
                owned.Property(x => x.AccountNumber).HasColumnName("AccountNumber").HasMaxLength(30);
                owned.Property(x => x.BranchCode).HasColumnName("BranchCode").HasMaxLength(20);
                owned.Property(x => x.AccountType).HasColumnName("AccountType").HasMaxLength(30);
            });
            entity.OwnsOne(x => x.TaxProfile, owned =>
            {
                owned.Property(x => x.TaxNumber).HasColumnName("TaxNumber").HasMaxLength(30);
                owned.Property(x => x.IncomeTaxReference).HasColumnName("IncomeTaxReference").HasMaxLength(30);
                owned.Property(x => x.IsUifExempt).HasColumnName("IsUifExempt");
                owned.Property(x => x.IsSdlExempt).HasColumnName("IsSdlExempt");
            });
        });

        modelBuilder.Entity<LeaveBalance>(entity =>
        {
            entity.ToTable("LeaveBalances");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.AnnualLeaveDays).HasColumnType("decimal(10,2)");
            entity.Property(x => x.SickLeaveDays).HasColumnType("decimal(10,2)");
            entity.Property(x => x.UnpaidLeaveDays).HasColumnType("decimal(10,2)");
            entity.Property(x => x.CarriedForwardDays).HasColumnType("decimal(10,2)");
        });

        modelBuilder.Entity<SalaryComponent>(entity =>
        {
            entity.ToTable("SalaryComponents");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            entity.HasOne(x => x.Employee)
                .WithMany(x => x.SalaryComponents)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PayrollRun>(entity =>
        {
            entity.ToTable("PayrollRuns");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.RunNumber).HasMaxLength(50).IsRequired();
            entity.Property(x => x.GrossTotal).HasColumnType("decimal(18,2)");
            entity.Property(x => x.TotalDeductions).HasColumnType("decimal(18,2)");
            entity.Property(x => x.NetTotal).HasColumnType("decimal(18,2)");
            entity.HasMany(x => x.PayrollLines)
                .WithOne(x => x.PayrollRun)
                .HasForeignKey(x => x.PayrollRunId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PayrollLine>(entity =>
        {
            entity.ToTable("PayrollLines");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.EmployeeName).HasMaxLength(160).IsRequired();
            entity.Property(x => x.GrossPay).HasColumnType("decimal(18,2)");
            entity.Property(x => x.TaxableEarnings).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Paye).HasColumnType("decimal(18,2)");
            entity.Property(x => x.UifEmployee).HasColumnType("decimal(18,2)");
            entity.Property(x => x.SdlEmployer).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CustomDeductions).HasColumnType("decimal(18,2)");
            entity.Property(x => x.NetPay).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Notes).HasMaxLength(4000);
            entity.HasOne(x => x.Employee)
                .WithMany(x => x.PayrollLines)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedUtc = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedUtc = now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

