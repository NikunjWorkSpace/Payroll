using Microsoft.Extensions.Options;
using SA.Payroll.Application.Abstractions;
using SA.Payroll.Application.Authentication;

namespace SA.Payroll.Infrastructure.Services;

public sealed class DemoIdentityService : IIdentityService
{
    private readonly DemoUsersOptions _options;

    public DemoIdentityService(IOptions<DemoUsersOptions> options)
    {
        _options = options.Value;
    }

    public Task<AuthenticatedUser?> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var users = _options.Users.Count > 0 ? _options.Users : DemoUsersOptions.DefaultUsers;
        var matchedUser = users.FirstOrDefault(x =>
            string.Equals(x.Email, request.Email, StringComparison.OrdinalIgnoreCase) &&
            x.Password == request.Password);

        if (matchedUser is null)
        {
            return Task.FromResult<AuthenticatedUser?>(null);
        }

        return Task.FromResult<AuthenticatedUser?>(
            new AuthenticatedUser(
                matchedUser.UserId,
                matchedUser.FullName,
                matchedUser.Email,
                matchedUser.Role,
                matchedUser.ParentCompanyId,
                matchedUser.TenantId));
    }
}

