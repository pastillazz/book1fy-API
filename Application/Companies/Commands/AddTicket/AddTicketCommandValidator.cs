using FluentValidation;

namespace Application.Companies.Commands.AddTicket;

public class AddTicketCommandValidator : AbstractValidator<AddTicketCommand>
{
    public AddTicketCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.ServiceId)
            .NotEmpty();

        RuleFor(x => x.StartTimeUtc)
            .NotEmpty();

        // The Service aggregate rejects this too, but as a 409; a reversed
        // range is a malformed request, so catch it here and return 400.
        RuleFor(x => x.EndTimeUtc)
            .NotEmpty()
            .GreaterThan(x => x.StartTimeUtc)
            .WithMessage("End time must be later than start time.");
    }
}
