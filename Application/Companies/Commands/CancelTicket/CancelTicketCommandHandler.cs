using Application.Abstractions.Authentication;
using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Repositories;

namespace Application.Companies.Commands.CancelTicket;

public class CancelTicketCommandHandler(ICompanyRepository companyRepository, IUserContext userContext, IUnitOfWork unitOfWork):ICommandHandler<CancelTicketCommand>
{
    public async Task<Result> Handle(CancelTicketCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository
            .GetCompleteByIdAsync(request.CompanyId, request.ServiceId,
                cancellationToken);

        if (company is null) return CompanyErrors.CompanyNotFound;

        var ticket = company.Services
            .FirstOrDefault(s => s.Id == request.ServiceId)?.Tickets
            .FirstOrDefault(t => t.Id == request.TicketId);

        var isCompanyOwner = company.OwnerId == userContext.UserId;
        var isTicketHolder = ticket is not null && ticket.UserId == userContext.UserId;

        if (!isCompanyOwner && !isTicketHolder) return CompanyErrors.NotOwner;

        var result=company.CancelTicket(request.ServiceId, request.TicketId);
        if (result.IsFailure) return result.Error!;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}