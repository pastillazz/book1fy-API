using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;

namespace Application.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateCompanyCommand>
{    
    public async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        if (await companyRepository.ExistsByIdAsync(request.Id, cancellationToken)) 
            return CompanyErrors.CompanyAlreadyExists;

        if (await companyRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return CompanyErrors.CompanyAlreadyExists;
        
        var companyCreated=Company.Create(request.Id,request.Name,
            request.Description,request.Email);
        if(companyCreated.IsFailure)return companyCreated.Error!;
        companyRepository.Add(companyCreated.Value);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}