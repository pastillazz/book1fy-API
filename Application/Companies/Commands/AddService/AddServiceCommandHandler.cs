using Application.Abstractions.Authentication;
using Application.Abstractions.Interfaces;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Companies.Commands.AddService;

public class AddServiceCommandHandler(
    ICompanyRepository companyRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork):ICommandHandler<AddServiceCommand,Guid>
{
    public async Task<Result<Guid>> Handle(AddServiceCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository
            .GetByIdAsync(request.CompanyId, cancellationToken);

        if (company is  null) return CompanyErrors.CompanyNotFound;

        if (company.OwnerId != userContext.UserId)
            return CompanyErrors.NotOwner;

        var result=company.AddService( request.Name, 
            request.Description, request.OpeningTime,
            request.ClosingTime, request.WorkDays, request.Price);
        
        if (result.IsFailure)
        {
            return result.Error;
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result.Value.Id;
    }
}