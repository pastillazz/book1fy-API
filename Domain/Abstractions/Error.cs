namespace Domain.Abstractions;

public sealed record Error(string Code, string? Message = null)
{
    public static readonly Error None = new (string.Empty);
   
}
