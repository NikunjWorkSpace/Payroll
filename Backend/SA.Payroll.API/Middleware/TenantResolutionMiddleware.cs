using SA.Payroll.Application.Abstractions;

namespace SA.Payroll.API.Middleware;

public sealed class TenantResolutionMiddleware
{
    private const string TenantHeaderName = "X-Tenant-Id";

    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext, ICurrentUserService currentUserService)
    {
        Guid? tenantId = null;

        if (context.Request.Headers.TryGetValue(TenantHeaderName, out var headerValue) &&
            Guid.TryParse(headerValue.FirstOrDefault(), out var parsedHeaderTenant))
        {
            tenantId = parsedHeaderTenant;
        }
        else
        {
            tenantId = currentUserService.TenantId;
        }

        tenantContext.SetTenant(tenantId);
        await _next(context);
    }
}

