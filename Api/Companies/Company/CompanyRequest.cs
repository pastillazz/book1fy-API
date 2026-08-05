namespace Api.Companies.Company;

public record CompanyRequest(
    string Name,
    string Description,
    string Email);