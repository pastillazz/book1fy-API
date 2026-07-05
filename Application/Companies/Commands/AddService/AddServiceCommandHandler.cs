using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Repositories;

namespace Application.Companies.Commands.AddService;

public class AddServiceCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork):ICommandHandler<AddServiceCommand>
{
    public async Task<Result> Handle(AddServiceCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByIdAsync(request.CompanyId, cancellationToken);
        if (company is null)
        {
            return Error.None;
        }
        
        var result=company.AddService(request.Id, request.Name, 
            request.Description, request.OpeningTime,
            request.ClosingTime, request.WorkDays, request.Price);
        
        if (result.IsSuccess)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        return Result.Failure(result.Error);
    }
}