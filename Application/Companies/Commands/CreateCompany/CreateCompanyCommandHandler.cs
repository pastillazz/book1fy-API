using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Companies.Commands;

public class CreateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateCompanyCommand>
{    
    public async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company=Company.Create(request.Id,request.Name,request.Description);
        await companyRepository.AddAsync(company, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}