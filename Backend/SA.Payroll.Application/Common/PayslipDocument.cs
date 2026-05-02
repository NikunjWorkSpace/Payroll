namespace SA.Payroll.Application.Common;

public sealed record PayslipDocument(
    string FileName,
    string ContentType,
    byte[] Content);

