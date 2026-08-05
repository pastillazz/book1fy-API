using System.Net;

namespace Domain.Abstractions;

public sealed record Error (
    string Code, 
    string? Message = null,
    HttpStatusCode StatusCode = HttpStatusCode.BadRequest)
{
    public static readonly Error None = new (string.Empty);
   
}
