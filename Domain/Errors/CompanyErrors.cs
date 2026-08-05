using System.Net;
using Domain.Abstractions;

namespace Domain.Errors;

public class CompanyErrors
{
    public static readonly Error CompanyNotFound = new("Company.NotFound",
        "Company was not found.",HttpStatusCode.NotFound);
    
    public static readonly Error CompanyAlreadyExists = new("Company.AlreadyExists",
        "Company with the same id already exists.",
        HttpStatusCode.Conflict);
}