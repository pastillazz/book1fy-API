using Domain.Abstractions;

namespace Domain.ValueObjects.Errors;

public class CompanyErrors
{
    public static readonly Error CompanyNotFound = new("Company.NotFound",
        "Company was not found.");
    
    public static readonly Error CompanyAlreadyExists = new("Company.AlreadyExists",
        "Company with the same id already exists.");
}