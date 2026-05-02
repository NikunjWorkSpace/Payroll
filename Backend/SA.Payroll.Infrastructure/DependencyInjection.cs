using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SA.Payroll.Application.Abstractions;
using SA.Payroll.Infrastructure.Persistence;
using SA.Payroll.Infrastructure.Services;

namespace SA.Payroll.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<DemoUsersOptions>(configuration.GetSection(DemoUsersOptions.SectionName));

        services.AddDbContext<PayrollDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(PayrollDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<PayrollDbContext>());
        services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<IPdfRenderer, HtmlBytesPdfRenderer>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IIdentityService, DemoIdentityService>();

        return services;
    }
}

