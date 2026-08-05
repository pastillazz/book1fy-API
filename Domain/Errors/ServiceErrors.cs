using System.Net;
using Domain.Abstractions;

namespace Domain.Errors;

public class ServiceErrors
{
    public static readonly Error NotFound= new ("Service.NotFound",
        "Service was not found.",HttpStatusCode.NotFound);
}