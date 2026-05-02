using SA.Payroll.Application.Abstractions;

namespace SA.Payroll.Infrastructure.Services;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
