using FluentValidation;

namespace Application.Companies.Queries.GetTicketById;

public class GetTicketByIdQueryValidator : AbstractValidator<GetTicketByIdQuery>
{
    public GetTicketByIdQueryValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.ServiceId)
            .NotEmpty();

        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}
