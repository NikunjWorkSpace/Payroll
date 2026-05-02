namespace SA.Payroll.Infrastructure.Services;

public sealed class DemoUsersOptions
{
    public const string SectionName = "DemoUsers";

    public List<DemoUserCredential> Users { get; set; } = new();

    public static IReadOnlyList<DemoUserCredential> DefaultUsers =>
        new List<DemoUserCredential>
        {
            new()
            {
                UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                FullName = "Global Administrator",
                Email = "global.admin@paybridge.co.za",
                Password = "Pass@123",
                Role = "GlobalAdmin",
                ParentCompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            },
            new()
            {
                UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                FullName = "Company Administrator",
                Email = "company.admin@paybridge.co.za",
                Password = "Pass@123",
                Role = "CompanyAdmin",
                ParentCompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                TenantId = Guid.Parse("22222222-2222-2222-2222-222222222222")
            },
            new()
            {
                UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                FullName = "Payroll Officer",
                Email = "payroll.officer@paybridge.co.za",
                Password = "Pass@123",
                Role = "PayrollOfficer",
                ParentCompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                TenantId = Guid.Parse("22222222-2222-2222-2222-222222222222")
            },
            new()
            {
                UserId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                FullName = "Employee User",
                Email = "employee@paybridge.co.za",
                Password = "Pass@123",
                Role = "Employee",
                ParentCompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                TenantId = Guid.Parse("22222222-2222-2222-2222-222222222222")
            }
        };
}

