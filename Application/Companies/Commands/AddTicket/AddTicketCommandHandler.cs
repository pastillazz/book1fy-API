using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Interfaces;
using Application.Common.Exceptions;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Companies.Commands.AddTicket;

public class AddTicketCommandHandler
(ICompanyRepository companyRepository, IUserContext userContext, IUnitOfWork unitOfWork)
    : ICommandHandler<AddTicketCommand,Guid>
{
    private const int MaxRetryCount = 3;
    public async Task<Result<Guid>> Handle(AddTicketCommand request, CancellationToken cancellationToken)
    { 
       for (int i = 0; i < MaxRetryCount;){

           try
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
           catch (ConcurrencyConflictException)
           {
               i++;
           }
       }

       return TicketErrors.OverlappingTicket;
    }
}
