namespace SA.Payroll.Application.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

