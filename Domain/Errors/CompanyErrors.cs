using System.Net;
using Domain.Shared;

namespace Domain.Errors;

public class CompanyErrors
{
    public static readonly Error CompanyNotFound = new("Company.NotFound",
        "Company was not found.",HttpStatusCode.NotFound);
    
    public static readonly Error CompanyAlreadyExists = new("Company.AlreadyExists",
        "Company with the same id already exists.",
        HttpStatusCode.Conflict);

    public static readonly Error NotOwner = new("Company.NotOwner",
        "You do not have permission to operate on this company.",
        HttpStatusCode.Forbidden);

    public static readonly Error NameEmpty = new("Company.NameEmpty",
        "Company name cannot be empty.");
}
