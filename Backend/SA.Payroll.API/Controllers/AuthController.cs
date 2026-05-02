using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Payroll.Application.Abstractions;
using SA.Payroll.Application.Authentication;

namespace SA.Payroll.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public AuthController(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _identityService.AuthenticateAsync(request, cancellationToken);
        if (user is null)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        var token = _tokenService.GenerateToken(user);
        return Ok(new AuthResponse(token, user.FullName, user.Email, user.Role, user.ParentCompanyId, user.TenantId));
    }
}

