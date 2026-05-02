using System.Text;
using SA.Payroll.Application.Abstractions;

namespace SA.Payroll.Infrastructure.Services;

public sealed class HtmlBytesPdfRenderer : IPdfRenderer
{
    public Task<byte[]> RenderAsync(string html, CancellationToken cancellationToken = default) =>
        Task.FromResult(Encoding.UTF8.GetBytes(html));
}

