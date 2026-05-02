namespace SA.Payroll.Infrastructure.Services;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "PayBridge.API";

    public string Audience { get; set; } = "PayBridge.Client";

    public string Key { get; set; } = "PayBridge-Development-Key-Change-Me-Immediately";

    public int ExpiryMinutes { get; set; } = 120;
}

