using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Repositories;
using Domain.ValueObjects.Errors;

namespace Application.Companies.Commands.AddTicket;

public class AddTicketCommandHandler
(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<AddTicketCommand>
{
    public async Task<Result> Handle(AddTicketCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository
            .GetCompleteByIdAsync(request.CompanyId, request.ServiceId,
                cancellationToken);

        if (company == null) return CompanyErrors.CompanyNotFound;
        
        var result = company.AddTicketToService(request.Id,request.ServiceId, request.UserId,
           request.StartTimeUtc, request.EndTimeUtc);
       
        if (result.IsFailure) return result.Error!; 
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}