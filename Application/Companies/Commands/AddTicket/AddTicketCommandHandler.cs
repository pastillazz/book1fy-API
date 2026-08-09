using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Interfaces;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Companies.Commands.AddTicket;

public class AddTicketCommandHandler
(ICompanyRepository companyRepository, IUserContext userContext, IUnitOfWork unitOfWork)
    : ICommandHandler<AddTicketCommand,Guid>
{
    public async Task<Result<Guid>> Handle(AddTicketCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository
            .GetCompleteByIdAsync(request.CompanyId, request.ServiceId,
                cancellationToken);

        if (company is null) return CompanyErrors.CompanyNotFound;

        var result = company.AddTicketToService(request.ServiceId, 
            userContext.UserId, request.StartTimeUtc, request.EndTimeUtc);
       
        if (result.IsFailure) return result.Error; 
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result.Value.Id;
    }
}