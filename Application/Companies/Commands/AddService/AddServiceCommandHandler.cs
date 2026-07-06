using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Repositories;
using Domain.ValueObjects.Errors;

namespace Application.Companies.Commands.AddService;

public class AddServiceCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork):ICommandHandler<AddServiceCommand>
{
    public async Task<Result> Handle(AddServiceCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository
            .GetByIdAsync(request.CompanyId, cancellationToken);
        
        if (company == null)
        {
            return CompanyErrors.CompanyNotFound;
        }
        
        var result=company.AddService(request.Id, request.Name, 
            request.Description, request.OpeningTime,
            request.ClosingTime, request.WorkDays, request.Price);
        
        if (result.IsFailure)
        {
            return result.Error!;
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}