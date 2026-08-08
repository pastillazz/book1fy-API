using FluentValidation;

namespace Application.Companies.Commands.CancelTicket;

public class CancelTicketCommandValidator : AbstractValidator<CancelTicketCommand>
{
    public CancelTicketCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.ServiceId)
            .NotEmpty();

        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}
