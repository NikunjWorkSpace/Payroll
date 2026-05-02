using SA.Payroll.Application.Authentication;

namespace SA.Payroll.Application.Abstractions;

public interface IIdentityService
{
    Task<AuthenticatedUser?> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default);
}

