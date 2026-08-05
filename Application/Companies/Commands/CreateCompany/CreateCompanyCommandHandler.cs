using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;

namespace Application.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateCompanyCommand,  Guid>
{    
    public async Task<Result<Guid>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyCreated=Company.Create(request.Name,
            request.Description,request.Email);
        if(companyCreated.IsFailure)return companyCreated.Error!;
        companyRepository.Add(companyCreated.Value);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return companyCreated.Value.Id;
    }
}