using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Repositories;

namespace Application.Companies.Commands.AddTicket;

public class AddTicketCommandHandler
(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<AddTicketCommand,Guid>
{
    public async Task<Result<Guid>> Handle(AddTicketCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository
            .GetCompleteByIdAsync(request.CompanyId, request.ServiceId,
                cancellationToken);

        if (company is null) return CompanyErrors.CompanyNotFound;
        
        var result = company.AddTicketToService(request.ServiceId, request.UserId,
           request.StartTimeUtc, request.EndTimeUtc);
       
        if (result.IsFailure) return result.Error!; 
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result.Value.Id;
    }
}