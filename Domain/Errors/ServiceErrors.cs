using Domain.Abstractions;

namespace Domain.ValueObjects.Errors;

public class ServiceErrors
{
    public static readonly Error NotFound= new ("Service.NotFound",
        "Service was not found.");
}