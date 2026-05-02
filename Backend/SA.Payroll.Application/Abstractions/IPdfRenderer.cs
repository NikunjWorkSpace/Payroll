namespace SA.Payroll.Application.Abstractions;

public interface IPdfRenderer
{
    Task<byte[]> RenderAsync(string html, CancellationToken cancellationToken = default);
}

