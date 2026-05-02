using SA.Payroll.Application.Authentication;

namespace SA.Payroll.Application.Abstractions;

public interface ITokenService
{
    string GenerateToken(AuthenticatedUser user);
}

