using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Repositories;

namespace Application.Companies.Commands.AddService;

public class AddServiceCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork):ICommandHandler<AddServiceCommand,Guid>
{
    public async Task<Result<Guid>> Handle(AddServiceCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository
            .GetByIdAsync(request.CompanyId, cancellationToken);
        
        if (company is  null) return CompanyErrors.CompanyNotFound;
        
        var result=company.AddService( request.Name, 
            request.Description, request.OpeningTime,
            request.ClosingTime, request.WorkDays, request.Price);
        
        if (result.IsFailure)
        {
            return result.Error!;
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result.Value.Id;
    }
}