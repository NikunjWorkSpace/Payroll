namespace SA.Payroll.Core.Common;

public interface ITenantOwned
{
    Guid TenantId { get; set; }
}

