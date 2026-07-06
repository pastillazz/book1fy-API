using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects.Errors;

namespace Application.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateCompanyCommand>
{    
    public async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var existingCompany=await companyRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if(existingCompany!=null)
        {
            return CompanyErrors.CompanyAlreadyExists;
        }
        
        var companyCreated=Company.Create(request.Id,request.Name,request.Description);
        companyRepository.Add(companyCreated);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}